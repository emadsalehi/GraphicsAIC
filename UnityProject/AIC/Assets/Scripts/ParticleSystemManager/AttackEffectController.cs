using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class dfsdf : MonoBehaviour
{
    public GameObject particleSystemGameObject;
    public float offsetLocationX;
    public float offsetLocationY;
    public float offsetLocationZ;
    public float distance;
    public Vector3 destination;
    public GameObject target;

    private ParticleSystem particeSystem;



    public void SetTarget(GameObject target)
    {
        if (target!=null){
            this.target = target;
        }
    }

    void Start()
    {   
        particleSystemGameObject.transform.position += new Vector3(offsetLocationX, offsetLocationY, offsetLocationZ);
        particeSystem = particleSystemGameObject.GetComponent<ParticleSystem>();
        if (target!=null){

        destination = target.transform.position;
        }
        
	}
  

    public void StopParticleSystem() {
        particeSystem.Stop();
    }

    public void PlayParticleSystem(Vector3 destination) {
        if (target!=null){
            destination -= particleSystemGameObject.transform.position;
            // distance = destination.magnitude;
            // var main = particeSystem.main;
            // float speed = main.startSpeed.Evaluate(0.0f);
            // main.startLifetime = distance / speed;
            particeSystem.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target!=null){
            Vector3 tp = target.transform.position;
            particleSystemGameObject.transform.LookAt(tp);
        }
    }
}
