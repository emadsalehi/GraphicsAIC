using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectController : MonoBehaviour
{
    public GameObject particleSystemGameObject;
    public float offsetLocationX;
    public float offsetLocationY;
    public float offsetLocationZ;
    public Vector3 destination;

    private GameObject target;
    private ParticleSystem particleSystem;
    private bool attackEnable = false;

    public void SetTarget(GameObject target)
    {
        if (target != null){
            this.target = target;
        }
    }

    void Start()
    {   
        particleSystemGameObject.transform.position += new Vector3(offsetLocationX, offsetLocationY, offsetLocationZ);
        particleSystem = particleSystemGameObject.GetComponent<ParticleSystem>();
        StopParticleSystem();
        if (target != null){
            destination = target.transform.position;
        }
    }
  

    public void StopParticleSystem() {
        particleSystem.Stop();
        attackEnable = false;
    }

    public void PlayParticleSystem(GameObject target)
    {
        this.target = target;
        if (this.target != null){
            destination = transform.position - particleSystemGameObject.transform.position;
            particleSystem.Play();
            attackEnable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackEnable) {
            Vector3 tp = target.transform.position;
            particleSystemGameObject.transform.LookAt(tp);
        }
    }
}
