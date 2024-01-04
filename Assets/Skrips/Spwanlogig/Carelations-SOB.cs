using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;


[CreateAssetMenu(fileName = "Car-", menuName = "ScriptableObjects/CarRealtions", order = 1)]
public class CarRealtions : ScriptableObject
{
    [SerializeField]
    public GameObject hiddenobjekt;
    [SerializeField]
    public List<GameObject> Raltionlist = new List<GameObject>();
    public GameObject getrendom(AnimationCurve curve)
    {
        List<float>Areas = new List<float>();
       int steps = Raltionlist.Count;
       float stepSize = curve.length / steps; //1

        float integral = 0f;
        for (int i = 0; i < steps; i++)
        {
            float t1 = i * stepSize;
            float t2 = (i + 1) * stepSize;

            float y1 = curve.Evaluate(t1);
            float y2 = curve.Evaluate(t2);

            float area = (y1 + y2) * 0.5f * stepSize;
            Areas.Add(area);
            integral += area;
        }
        float rendom = Random.Range(0, integral);

        int j = 0;
        for (float sum = 0; sum < rendom; j++){
            sum += Areas[j];
        }

        return Raltionlist[j];

    } 

}