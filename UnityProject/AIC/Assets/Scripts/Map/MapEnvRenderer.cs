using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvRenderer : MonoBehaviour
{
    public GameObject[] MapEnvs;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RenderMapEnv(int height, int width)
    {
        String envName = height + "by" + width + " Normalized";
        foreach (GameObject mapEnv in MapEnvs)
        {
            if (mapEnv.name == envName)
            {
                Instantiate(mapEnv);
                break;
            }
        }
    }
}
