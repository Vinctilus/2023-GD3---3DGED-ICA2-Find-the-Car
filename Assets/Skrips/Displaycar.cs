using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Displaycar : MonoBehaviour
{
    [SerializeField]
    float timeforfullRotation = 5;
    float timepast = 0;

    GameObject Car;
    void Update()
    {
        timepast += Time.deltaTime;
        if(timepast > timeforfullRotation)
        {
            timepast -= timeforfullRotation;
            
        }
        transform.eulerAngles = new Vector3(0, 360 / timeforfullRotation * timepast, 0);
    }

    public void changeCar(GameObject newcar)
    {
        for (int b = 0; b < transform.childCount; b++)
        {
            Destroy(transform.GetChild(b).gameObject);
        }

        Car = Instantiate(newcar, transform.position, transform.rotation);
        Car.transform.parent = transform;
       

    }


}
