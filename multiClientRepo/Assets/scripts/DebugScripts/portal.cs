using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    kid kid;
    private void OnContact()
    {
        kid.Teleport();
    }
}
