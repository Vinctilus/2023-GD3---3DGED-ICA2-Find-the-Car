using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "DifficultyCurve", menuName = "Pettern/DifficultyCurve", order = 1)]
public class DifficultyCurve : ScriptableObject
{
    [SerializeField]
    public AnimationCurve curve;
}