using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveIntersection : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            if (other.gameObject.TryGetComponent<CarController>(out CarController carController))
            {
                carController.isInIntersection = false;
            }
        }
    }
}
