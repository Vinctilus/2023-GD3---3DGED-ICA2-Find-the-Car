using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oncolision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collison");
        Debug.Log("CS: " + collision.gameObject.tag);
        Debug.Log("DS: " + collision.collider.name);
    }
}
