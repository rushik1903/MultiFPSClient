using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float lifeTime=2f;
    public GameObject bulletSoundPrefab;
    private GameObject bulletSound;
    private void Start()
    {
        Invoke("KillBullet", lifeTime);
        bulletSound = Instantiate(bulletSoundPrefab, transform.position, Quaternion.identity);
    }
    private void OnCollisionEnter(Collision collisionInfo)
    {
        Destroy(gameObject);
    }

    private void KillBullet()
    {
        Destroy(gameObject);
    }
}
