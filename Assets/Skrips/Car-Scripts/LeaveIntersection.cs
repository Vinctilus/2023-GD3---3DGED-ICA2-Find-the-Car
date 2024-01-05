using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_leafe_turn : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            if (other.gameObject.TryGetComponent<CarController>(out CarController b))
            {
                b.isInIntersection = false;
            }

        }

    }
}
