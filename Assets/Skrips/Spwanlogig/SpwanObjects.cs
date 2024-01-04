using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SpwanObjects : MonoBehaviour
{
    [SerializeField]
    LayerMask MaskCarLayer;
    [SerializeField]
    GameObject Debugqube;
    [SerializeField]
    public bool blocked = false;
    [SerializeField]
    float dontspwen = 16f;

    private void Update()
    {
        RaycastHit hit;
        blocked = false;
        if (Physics.Raycast(transform.position + transform.forward, -transform.forward, out hit, dontspwen*2, MaskCarLayer)) { blocked = true;} 
        else if (Physics.Raycast(transform.position - transform.forward, transform.forward, out hit, dontspwen*2, MaskCarLayer)) { blocked = true;}
        else if(Physics.Raycast(transform.position - transform.forward* (dontspwen/2), transform.forward, out hit, dontspwen * 2, MaskCarLayer)) { blocked = true;}


       

    }

    public Transform spwancar()
    {
        RaycastHit hit;
        Transform data = null;
        bool hitnothing = true;
     

        if (hitnothing&&!blocked) 
        { 
            
            data = transform;
            blocked = true;
        }

        return data;
        
    }
}
