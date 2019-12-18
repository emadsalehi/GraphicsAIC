using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomizedPS 
{
    public string name;

    [Range(0f, 10f)]
    public float time;

    public GameObject gameObject;

    public CustomizedPS(CustomizedPS cps) {
        this.name = cps.name;
        this.time = cps.time;
    }

    public void Update() {
        this.time -= Time.deltaTime;
        if(time <= 0) {
            this.GetParticleSystem().Stop();
        }
    }

    public bool isFinished() {
        return time <= 0 && !this.GetParticleSystem().IsAlive();
    }

    public ParticleSystem GetParticleSystem() {
        return gameObject.GetComponent<ParticleSystem>();
    }

}