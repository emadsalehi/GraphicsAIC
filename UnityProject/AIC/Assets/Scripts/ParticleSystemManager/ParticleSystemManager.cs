using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{

    public static ParticleSystemManager instance;
    public CustomizedPS[] systems;
    private static List<CustomizedPS> playingSystems = new List<CustomizedPS>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Play("sadra", new Vector3(10, 10, 10));
        }
        playingSystems.ForEach(element => element.Update());
        deleteFinishedSystems();

    }

    public void Play(string name, Vector3 position)
    {
        CustomizedPS temp = Array.Find(systems, item => item.name == name);
        if (temp == null)
        {
            Debug.LogError("Particle System: " + name + "not found!");
            return;
        }
        playingSystems.Add(cloneCPS(temp, position, temp.GetParticleSystem().transform.rotation));
    }

    public void Play(string name,  Vector3 position, Quaternion rotation)
    {
        CustomizedPS temp = Array.Find(systems, item => item.name == name);
        if (temp == null)
        { 
            Debug.LogError("Particle System: " + name + "not found!");
            return;
        }

        // if(rotation == null) {
        //     rotation = (Quaternion)temp.GetParticleSystem().transform.rotation;
        //     playingSystems.Add(cloneCPS(temp, position, temp.GetParticleSystem().transform.rotation));
        // }

        playingSystems.Add(cloneCPS(temp, position, rotation));
    }


    private static CustomizedPS cloneCPS(CustomizedPS cps, Vector3 position, Quaternion rotation)
    {
        CustomizedPS result = new CustomizedPS(cps);
        result.gameObject = (GameObject)Instantiate(cps.gameObject, position, rotation);
        return result;
    }

    private static void deleteFinishedSystems()
    {
        for (int i = 0; i < playingSystems.Count; i++)
        {
            if (playingSystems[i].isFinished())
            {
                CustomizedPS temp = playingSystems[i];
                playingSystems.RemoveAt(i);
                i--;
                Destroy(temp.gameObject);
            }
        }
    }
}
