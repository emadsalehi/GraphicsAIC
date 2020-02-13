using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public List<GameObject> cameras;

    private int _totalCameras;
    private int _activeCamera = 0;

    void Start()
    {
        _totalCameras = cameras.Count;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_activeCamera == _totalCameras - 1)
                _activeCamera = 0;
            else
                _activeCamera++;
            cameras[_activeCamera].SetActive(true);
            for (var i = 0 ; i < _totalCameras ; i++)
            {
                cameras[i].SetActive(_activeCamera == i);
            }
        }
    }
}
