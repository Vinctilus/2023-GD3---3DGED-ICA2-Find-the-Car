using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Dificulty", menuName = "Pettern/Dificulty", order = 1)]
public class Dificulty : ScriptableObject
{
    [SerializeField]
    public AnimationCurve curve;
}