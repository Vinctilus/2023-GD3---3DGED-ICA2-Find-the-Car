using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "Car-", menuName = "ScriptableObjects/CarRelationConnections", order = 1)]
public class CarRelationConnections : ScriptableObject
{
    [Header("Car")]
    [SerializeField]
    public GameObject hiddenobjekt;
    [Header("List from Similar to Unsimilar")]
    [SerializeField]
    public List<GameObject> Raltionlist = new List<GameObject>();

    // This is a Random Algorithm that depends on a Curve.
    // It uses the Volume under the Curve for the Possibility
    // of the Index.
    // The sum would be enough for the approximation, but I built it with the Volume.
    public GameObject GetRandom(AnimationCurve curve)
    {
       List<float>Areas = new List<float>();
       int steps = Raltionlist.Count;
       float stepSize = curve.length /((float)steps); //1

        float integral = 0f;
        for (int i = 0; i < steps-1; i++)
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