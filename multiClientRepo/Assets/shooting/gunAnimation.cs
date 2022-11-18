using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAnimation : MonoBehaviour
{
    private Animator gunAnimator;
    public AudioClip shootSound;
    public AudioSource shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        gunAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Shoot()
    {
        gunAnimator.SetTrigger("shoot");
        //shootPoint.PlayOneShot(shootSound);
    }

    public void Reload()
    {
        gunAnimator.SetTrigger("reload");
    }

    //public void Idle()
    //{
    //    gunAnimator.SetTrigger("idle");
    //}
}
