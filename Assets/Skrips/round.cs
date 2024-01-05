using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject obj = transform.GetChild(i).gameObject;
            Vector3 v = obj.transform.position;
            obj.transform.position = new Vector3(Mathf.Round(v.x/10)*10, 5, Mathf.Round(v.z / 10) * 10);
        }
    }


}
