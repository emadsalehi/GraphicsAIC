using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectController : MonoBehaviour
{
    public GameObject particleSystemGameObject;

    private Vector3 destination;
    private GameObject target;
    private ParticleSystem particleSystem;
    private bool attackEnable = false;

    void Start()
    {   
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
            particleSystemGameObject.transform.LookAt(target.transform, Vector3.forward);
        }
    }
}
