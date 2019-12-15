using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        Vector3 pos = transform.position;
        Camera camera = GetComponent<Camera>();
        Debug.Log("or: " +camera.orthographicSize);
        Debug.Log("hei: " + Screen.height);
        Debug.Log("wid: " + Screen.width);
        Debug.Log("aspec: " + aspectRatio);
        //transform.position = new Vector3(pos.x + camera.orthographicSize * aspectRatio, pos.y + camera.orthographicSize, pos.z);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
