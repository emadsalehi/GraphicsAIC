using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounder : MonoBehaviour
{
    public int[] xBounds = new int[2];
    public int[] yBounds = new int[2];
    public int[] zBounds = new int[2];

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        if (position.x < xBounds[0])
        {
            transform.position = new Vector3(xBounds[0], position.y, position.z);
        } 
        if (position.x > xBounds[1])
        {
            transform.position = new Vector3(xBounds[1], position.y, position.z);
        }
        if (position.y < yBounds[0])
        {
            transform.position = new Vector3(position.x, yBounds[0], position.z);
        } 
        if (position.y > yBounds[1])
        {
            transform.position = new Vector3(position.x, yBounds[1], position.z);
        }
        if (position.z < zBounds[0])
        {
            transform.position = new Vector3(position.x, position.y, zBounds[0]);
        } 
        if (position.z > zBounds[1])
        {
            transform.position = new Vector3(position.x, position.y, zBounds[1]);
        }
    }
}
