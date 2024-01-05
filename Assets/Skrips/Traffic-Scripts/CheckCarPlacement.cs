using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CheckCarPlacement : MonoBehaviour
{
    [Header("Get Layer")]
    [SerializeField]
    LayerMask carLayerMask;
    [Header("Settings")]
    [SerializeField]
    public bool isBlocked = false;
    [SerializeField]
    float doNotSpawnInDistance = 16f;

    private void Update()
    {
        RaycastHit hit;
        isBlocked = false;
        if (Physics.Raycast(transform.position + transform.forward, -transform.forward, out hit, doNotSpawnInDistance*2, carLayerMask)) { isBlocked = true;} 
        else if (Physics.Raycast(transform.position - transform.forward, transform.forward, out hit, doNotSpawnInDistance*2, carLayerMask)) { isBlocked = true;}
        else if(Physics.Raycast(transform.position - transform.forward* (doNotSpawnInDistance/2), transform.forward, out hit, doNotSpawnInDistance * 2, carLayerMask)) { isBlocked = true;}


       

    }

    public Transform spwancar()
    {
        Transform data = null;
        bool hitnothing = true;
     

        if (hitnothing&&!isBlocked) 
        { 
            
            data = transform;
            isBlocked = true;
        }

        return data;
        
    }
}
