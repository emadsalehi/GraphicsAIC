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
        if (GetComponent<Collider>().transform.position.y > 5 * GetComponent<Collider>().bounds.size.y)
            return;
        rigidBody.velocity = new Vector3(0, 0, 0);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}
