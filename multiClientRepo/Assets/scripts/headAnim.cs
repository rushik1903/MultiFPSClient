using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headAnim : MonoBehaviour
{
    Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.material = gameObject.GetComponentInParent<Material>();
    }
}
