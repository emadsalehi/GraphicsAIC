using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    public float speed = 5.0f;
    public int[] xThresholds;
    public int[] yThresholds;
    public int[] zThresholds;
    
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x <= xThresholds[1])
                transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x >= xThresholds[0])
                transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.z >= zThresholds[1])
                transform.Translate(new Vector3(0,0,-speed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.z <= zThresholds[0])
                transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
        }
    }
}
