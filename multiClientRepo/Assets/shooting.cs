using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RiptideNetworking;

public class shooting : MonoBehaviour
{
    private gunAnimation animatorScript;
    public GameObject bullet;
    public float shootForce = 1000, shootVelocity=400, upwardForce;
    public int gunType=0;

    public float timeBetweenShooting = 0.1f, spread=0f, reloadTime=1.5f, timeBetweenShots=0;
    public int magazineSize = 30, bulletsPerTap = 1;
    public bool allowButtonHold = true;
    private int bulletsLeft, bulletsShot;

    private bool firing, readyToFire, reloading;

    public Camera fpsCam;
    public Transform attackPoint;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke =true;

    private void Start()
    {
        animatorScript = gameObject.GetComponent<gunAnimation>();
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToFire = true;
    }

    private void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
        {
            //25/30 format
            //ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);

            //just 25 format
            ammunitionDisplay.SetText((bulletsLeft / bulletsPerTap).ToString());
            if(bulletsLeft == 0)
            {
                ammunitionDisplay.SetText("R");
            }
        }
    }

    private void MyInput()
    {
        if (allowButtonHold) firing = Input.GetKey(KeyCode.Mouse0);
        else firing = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) { Reload(); }

        if(readyToFire && !reloading && bulletsLeft <= 0) { Reload(); }

        if(readyToFire && firing && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToFire = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 hitPoint;
        if(Physics.Raycast(ray,out hit))
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = ray.GetPoint(75);  //a random far point from cam
        }

        //guns attack point to hitpoint
        Vector3 directionWithoutSpread = hitPoint - attackPoint.position;

        //calc bullet spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //adding spread to bullet
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        animatorScript.Shoot();

        //using impulse to give bullet velocity

        //Adding force to bullet
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce,ForceMode.Impulse);
        //below line for grenades
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //giving veloecity directly to bullet(to send velocity to the server, impulse takes some time to acc the bullet)
        currentBullet.GetComponent<Rigidbody>().velocity = directionWithSpread.normalized * shootVelocity;

        SendShoot(currentBullet.transform.position, currentBullet.GetComponent<Rigidbody>().velocity);

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShoot", timeBetweenShooting);
            allowInvoke = false;
        }

        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShoot()
    {
        readyToFire = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
        animatorScript.Reload();
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void SendShoot(Vector3 position, Vector3 velocity)
    {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ClientToServerId.playerShoot);
        message.AddVector3(position);
        message.AddVector3(velocity);
        message.AddInt(gunType);
        NetworkManager.Singleton.Client.Send(message);
    }
}
