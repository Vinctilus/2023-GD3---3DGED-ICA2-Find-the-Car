using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class DisplayCar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    float timeForFullRotation = 15;
    [Header("Debug")]
    [SerializeField]
    float timePast = 0;
    [SerializeField]
    GameObject car;
    void Update()
    {
        timePast += Time.deltaTime;
        if(timePast > timeForFullRotation)
        {
            timePast -= timeForFullRotation;
            
        }
        transform.eulerAngles = new Vector3(0, 360 / timeForFullRotation * timePast, 0);
    }

    public void ChangeCar(GameObject newcar)
    {
        for (int b = 0; b < transform.childCount; b++)
        {
            Destroy(transform.GetChild(b).gameObject);
        }

        car = Instantiate(newcar, transform.position, transform.rotation);
        car.transform.parent = transform;
       

    }


}
