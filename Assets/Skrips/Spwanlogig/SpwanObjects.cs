using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SpwanObjects : MonoBehaviour
{
    public Transform spwancar()
    {
        RaycastHit hit;
        Transform data = null;
        Ray ray = new Ray(transform.position, Vector3.zero);
        bool hitnothing = !Physics.SphereCast(transform.position - transform.forward * 0.5f, 25f, transform.forward, out hit, 1f, LayerMask.GetMask("Cars"));

        
        if (hitnothing) 
        { 
            
            data = transform;
        }

        return data;
        
    }
}
