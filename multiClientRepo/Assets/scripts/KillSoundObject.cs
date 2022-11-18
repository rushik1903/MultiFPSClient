using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSoundObject : MonoBehaviour
{
    private float lifeTime = 2;
    void Start()
    {
        Invoke("KillSound", lifeTime);
    }

    void KillSound()
    {
        Destroy(gameObject);
    }
}
