using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{

    private Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision collision)
    {
        rigidBody.transform.position = new Vector3(rigidBody.transform.position.x, 0, rigidBody.transform.position.z);
        rigidBody.velocity = new Vector3(0, 0, 0);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}
