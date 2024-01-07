using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class bandingpositone : MonoBehaviour
{
    [SerializeField]
    Vector3 startTransform;
    [SerializeField]
    public float amound;
    [SerializeField]
    SphereManager spheremanager;
    // Start is called before the first frame update
    void Awake()
    {

        startTransform = transform.position;
        spheremanager = GameObject.FindGameObjectWithTag("spheremanager").GetComponent<SphereManager>();
    }

    // Update is called once per frame
    void Update()
    {
        amound = spheremanager.bendingAmount;
        if (amound > 0)
        {
            Vector3 camera = Camera.main.transform.position;
            Vector3 position = startTransform - camera;
            float a = mathpart(position.x);
            float b = mathpart(position.z);
            gameObject.transform.position = startTransform+ new Vector3(0, a + b, 0);
        }
        else
        {
            gameObject.transform.position = startTransform;
        }
} 
    float mathpart(float x)
    {
        return x*x* -amound;
    }

}
