using System.Collections.Generic;
using UnityEngine;

public class PathNote : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool dontNeedInTurn = false;
    [SerializeField] bool straight = false;
    [SerializeField] bool right = false;
    [SerializeField] bool left = false;

    [Header("Get Layer")]
    [SerializeField] LayerMask layerMaskPathNote;
    [SerializeField] LayerMask layerMaskCars;

    [Header("Debug")]
    [SerializeField] Vector4 straightV;
    [SerializeField] Vector4 rightV;
    [SerializeField] Vector4 leftV;

    // 0:left, 1:straight, 2:right
    [SerializeField] private Dictionary<int, List<Vector4>> possibleDirections = new Dictionary<int, List<Vector4>>();

    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;

        Vector3 offset = Kvd(transform.position - parentObject.transform.position);
        float stopline = 0;
        float center = 0;

        if (offset.x > offset.z)
        {
            center = offset.z;
            stopline = offset.x;
        }
        else
        {
            center = offset.x;
            stopline = offset.z;
        }

        if (straight)
        {
            possibleDirections.Add(3, new List<Vector4> { Prio(transform.position + Filter(offset, transform.forward * 1f), 3) });
            possibleDirections[3].Add(Prio(transform.position + Filter(offset, transform.forward * 2f), 3));

            straightV = SearchForGoal(transform.forward, possibleDirections[3][1], 3);
            possibleDirections[3].Add(Prio(transform.forward, -1));
        }

        if (right)
        {
            possibleDirections.Add(4, new List<Vector4> { Prio(transform.position + Filter(offset, transform.forward * ((stopline - center) / stopline)), 4) });
            possibleDirections[4].Add(Prio(transform.position + Filter(offset, transform.forward * ((stopline - center) / stopline) + transform.right * ((stopline - center) / center)), 4));

            rightV = SearchForGoal(transform.right, possibleDirections[4][1], 4);
            possibleDirections[4].Add(Prio(transform.right, -1));
        }

        if (left)
        {
            possibleDirections.Add(2, new List<Vector4> { Prio(transform.position + Filter(offset, transform.forward * 1.2f - transform.right * 0.8f), 2) });
            possibleDirections[2].Add(Prio(transform.position + Filter(offset, transform.forward * ((stopline + center) / stopline) - transform.right * ((stopline + center) / center)), 2));

            leftV = SearchForGoal(-transform.right, possibleDirections[2][1], 2);
            possibleDirections[2].Add(Prio(-transform.right, -1));
        }
    }

    void Update()
    {
        if (possibleDirections.Count == 0) { Start(); }
    }

    private Vector4 SearchForGoal(Vector3 direction, Vector3 position, int x)
    {
        float rayLength = 1000f;
        Ray ray = new Ray(position - direction * 10f, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, layerMaskPathNote))
        {
            Vector4 temp = Prio(hit.transform.position + direction * 5, 0);
            possibleDirections[x].Add(temp);
            return temp;
        }
        else
        {
            return Vector4.zero;
        }
    }

    public List<Vector4> GetGoal(int a)
    {
        if (!IsAKey(a))
        {
            a = RendomKey(Random.Range(0, possibleDirections.Count));
        }

        if (IsAKey(a))
        {
            List<Vector4> positions = possibleDirections[a];

            if (!dontNeedInTurn && possibleDirections.Count > 1)
            {
                Vector3 direction = positions[3];
                Vector3 from = positions[1];
                Ray ray = new Ray(from - direction * 10f, direction);
                RaycastHit[] hits = Physics.SphereCastAll(ray, 2f, 10f, layerMaskCars);

                bool gotHit = false;
                int i = 0;

                foreach (var hit in hits)
                {
                    i++;

                    if (hit.transform.gameObject.TryGetComponent<CarController>(out CarController setter))
                    {
                        if (!setter.ignoreCollision && setter.GetIfItIsWaiting())
                        {
                            gotHit = true;
                        }
                    }
                }

                if (gotHit)
                {
                    List<Vector4> otherPositions = GetOther(possibleDirections.Keys);

                    if (otherPositions != null)
                    {
                        positions = otherPositions;
                    }
                }
            }

            return new List<Vector4>() { positions[0], positions[1], positions[2] };
        }

        Debug.LogWarning("Wth wie kann das leer sein???");
        return null;
    }

    public List<Vector4> GetOther(ICollection<int> keys)
    {
        foreach (var key in keys)
        {
            List<Vector4> positions = possibleDirections[key];
            Vector3 direction = positions[3];
            Vector3 from = positions[1];
            Ray ray = new Ray(from - direction * 10f, direction);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 2f, 10f, layerMaskCars);

            bool gotHit = false;
            int i = 0;

            foreach (var hit in hits)
            {
                i++;

                if (hit.transform.gameObject.TryGetComponent<CarController>(out CarController setter))
                {
                    if (!setter.ignoreCollision && setter.GetIfItIsWaiting())
                    {
                        gotHit = true;
                        break;
                    }
                }
            }

            if (!gotHit) { return positions; }
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<CarController>().lastPathNote = this;

        if (other.gameObject.CompareTag("Car"))
        {
            CarController setter = other.gameObject.GetComponent<CarController>();

            if (setter != null)
            {
                if (dontNeedInTurn)
                {
                    setter.InsertGoals(GetGoal(0));
                }
                else
                {
                    setter.priority = RendomKey(Random.Range(0, possibleDirections.Count));
                }
            }
        }
    }

    // Is filtering the Vector
    private Vector3 Filter(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    // Take the direction of the Vector
    private Vector4 Prio(Vector3 v1, float a)
    {
        return new Vector4(v1.x, v1.y, v1.z, a);
    }

    private Vector3 Kvd(Vector3 v1)
    {
        return new Vector3(Kd(v1.x), Kd(v1.y), Kd(v1.z));
    }

    // Do negative float to positive number
    private float Kd(float v)
    {
        return v < 0 ? -v : v;
    }

    int RendomKey(int a)
    {
        List<int> keyCollection = new List<int>();

        foreach (int key in possibleDirections.Keys)
        {
            keyCollection.Add(key);
        }

        int randomIndex = 0;

        if (keyCollection.Count > 1)
        {
            randomIndex = a % (keyCollection.Count);
        }

        return keyCollection[randomIndex];
    }

    bool IsAKey(int a)
    {
        return possibleDirections.ContainsKey(a);
    }
}
