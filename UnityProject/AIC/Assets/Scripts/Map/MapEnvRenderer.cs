using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvRenderer : MonoBehaviour
{
    public GameObject[] MapEnvs;

    public void RenderMapEnv(int width, int height)
    {
        var envName = width + "by" + height + " Normalized";
        var found = false;
        foreach (var mapEnv in MapEnvs)
        {
            if (mapEnv.name != envName) continue;
            found = true;
            Instantiate(mapEnv);
            break;
        }

        envName = "20by20 Normalized";
        if (found) return;
        {
            foreach (var mapEnv in MapEnvs)
            {
                if (mapEnv.name != envName) continue;
                Instantiate(mapEnv);
                break;
            }
        }
    }
}
