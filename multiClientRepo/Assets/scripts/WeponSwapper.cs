using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponSwapper : MonoBehaviour
{
    public GameObject rifle,burstRifle;
    public GameObject WeponParent,weponSpawnPosition;
    private GameObject currentWepon,nextWepon;
    public Camera fpsCam;
    public float RotationSpeed = 50;

    private bool swithchingCtoN = false, swithchingNtoC = false;

    private void Start()
    {
        currentWepon = Instantiate(rifle, WeponParent.transform);
        currentWepon.transform.position = WeponParent.transform.position;
        currentWepon.GetComponent<shooting>().fpsCam = fpsCam;

        nextWepon = Instantiate(burstRifle, WeponParent.transform);
        nextWepon.transform.position = WeponParent.transform.position;
        nextWepon.GetComponent<shooting>().fpsCam = fpsCam;
        nextWepon.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && currentWepon.activeInHierarchy) // forward
        {
            if (!swithchingCtoN)
            {
                nextWepon.SetActive(true);
                nextWepon.transform.Rotate(-90, 0, 0);
                Invoke("A", 18*Time.fixedDeltaTime);
            }
            swithchingCtoN = true;
            SwitchWeponNext();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && nextWepon.activeInHierarchy) // backwards
        {
            if (!swithchingNtoC)
            {
                currentWepon.SetActive(true);
                Invoke("B", 14 * Time.fixedDeltaTime);
            }
            swithchingNtoC = true;
            SwitchWeponBack();
        }

        if (swithchingCtoN)
        {
            SwitchWeponNext();
        }
        if (swithchingNtoC)
        {
            SwitchWeponBack();
        }
    }
    private void SwitchWeponNext()
    {
        currentWepon.transform.Rotate(Vector3.right * 5);
        nextWepon.transform.Rotate(Vector3.right * 5);
        Debug.Log(currentWepon.transform.localEulerAngles.x);
        //if (nextWepon.transform.localEulerAngles.x >= 0)
        //{
        //    nextWepon.transform.localEulerAngles = new Vector3(0, 0, 0);
        //    currentWepon.transform.localEulerAngles = new Vector3(90f, 0, 0);
        //    currentWepon.SetActive(false);
        //    swithchingCtoN = false;
        //}
    }

    private void SwitchWeponBack()
    {
        nextWepon.transform.Rotate(Vector3.left * 5);
        currentWepon.transform.Rotate(Vector3.left * 5);
        Debug.Log(nextWepon.transform.localEulerAngles.x);
        //if (currentWepon.transform.localEulerAngles.x==270)
        //{
        //    nextWepon.transform.localEulerAngles = new Vector3(270f, 0, 0);
        //    currentWepon.transform.localEulerAngles = new Vector3(0, 0, 0);
        //    nextWepon.SetActive(false);
        //    swithchingNtoC = false;
        //}
    }

    private void A()
    {
        nextWepon.transform.localEulerAngles = new Vector3(0, 0, 0);
        currentWepon.transform.localEulerAngles = new Vector3(90f, 0, 0);
        currentWepon.SetActive(false);
        swithchingCtoN = false;
    }

    private void B()
    {
        nextWepon.transform.localEulerAngles = new Vector3(270f, 0, 0);
        currentWepon.transform.localEulerAngles = new Vector3(0, 0, 0);
        nextWepon.SetActive(false);
        swithchingNtoC = false;
    }

}
