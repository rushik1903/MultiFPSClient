using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour {

    public float spawnEffectTime = 2;
    public AnimationCurve fadeIn;
    public GameObject parentObject;

    ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;

    int shaderProperty;

    bool startAnimation = false;

	void Start ()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
    }

    public void StartAnimation()
    {
        startAnimation = true;
    }
	
	void Update ()
    {
        if (!startAnimation) { return; }
        if (timer < spawnEffectTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Debug.Log(parentObject.name);
            //Invoke("ParentBack", 4f);
            timer = 0;
            startAnimation = false;
        }
        if (timer == 0)
        {
            return;
        }
        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(spawnEffectTime/2, spawnEffectTime, timer)));
        
    }

    public void AnimBackToVisible()
    {
        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(0));
    }
}
