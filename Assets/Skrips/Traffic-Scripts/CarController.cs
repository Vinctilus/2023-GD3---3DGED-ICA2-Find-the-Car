using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;

public class CarController : MonoBehaviour
{
    [Header("Get Layer")]
    [SerializeField]
    LayerMask layerMaskCorner;
    [SerializeField]
    LayerMask layerMaskCars;
    [SerializeField]
    LayerMask layerCarsOff;
    [Header("TrafficManager")]
    //Get set from TrafficManager themself
    [HideInInspector]
    public TrafficManager trafficmanager;

    [Header("Settings")]

    [SerializeField]
    float raycastCarsLength = 0;
    [SerializeField]
    private float raycastCarsSize = 5;
    [Header("Debuge:")]
    [Header("Infos")]
    [SerializeField]
    public bool hiddenObject = false;


    [Header("NAV System")]
    [SerializeField]
    NavMeshAgent agent;
    public Queue<Vector4> goals = new Queue<Vector4>();
    [SerializeField]
    int goalsLength = 0;
    [SerializeField]
    Vector3 stopStorage;
    [SerializeField]
    bool toggleStop = false;
    [Header("Traffic System")]
    [SerializeField]
    public int priority;
    [SerializeField]
    public bool isStop = false;
    [SerializeField]
    public bool isWait = false;
    [SerializeField]
    public bool isInIntersection;
    [SerializeField]
    public float inIntersectionTime = 0;
    [SerializeField]
    public PathNote lastPathNote;
    public IntersectionManager lastIntersection;
    [Header("Collision Detection")]
    [SerializeField]
    float angleDifference;
    [SerializeField]
    public bool ignoreCollision = false;
    [SerializeField]
    float simplifyRotation4 = 0;




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FillEmptyQueue();
    }

    private void FillEmptyQueue()
    {
        if (lastPathNote == null)
        {
            float rayLength = 1000f;
            Ray ray = new Ray(agent.transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength, layerMaskCorner))
            {
                CorrectPosition(hit.transform.position + transform.forward * 5);
            }
        }
        else
        {
            InsertGoals(lastPathNote.GetGoal(priority));
            goals.Dequeue();
            goals.Dequeue();
        }
    }

    void Update()
    {
        if (goals.Count == 0 && isInIntersection)
        {
            isInIntersection = false;
        }

        if (goals.Count == 1 && ignoreCollision)
        {
            ignoreCollision = false;
            gameObject.layer = LayerMask.NameToLayer("cars");
        }

        if (GetIfItIsWaiting() && isInIntersection)
        {
            inIntersectionTime += Time.deltaTime;
            if (inIntersectionTime >= 10)
            {
                lastIntersection.ByFullIntersection();
            }
        }
        else
        {
            inIntersectionTime = 0;
        }

        if (goals.Count == 0)
        {
            FillEmptyQueue();
        }

        simplifyRotation4 = Mathf.Round(transform.eulerAngles.y / 90);
        goalsLength = goals.Count;

        if (goals.Count == 0 && !isWait)
        {
            FillEmptyQueue();
        }

        if (agent.enabled && Vector3.Distance(agent.transform.position, agent.destination) <= 1.5 && goals.Count != 0)
        {
            Vector4 split = goals.Dequeue();
            stopStorage = new Vector3(split.x, split.y, split.z);
            agent.destination = new Vector3(split.x, split.y, split.z);
            agent.avoidancePriority = (int)split.w;
            priority = (int)split.w;
        }

        if (!ignoreCollision)
        {
            Ray ray = new Ray(agent.transform.position + transform.forward * (1f + (raycastCarsSize / 2)), transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(ray, raycastCarsSize, raycastCarsLength, layerMaskCars);

            isStop = false;
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject != gameObject)
                {
                    float min = 0; float max = 0;
                    switch (priority)
                    {
                        case 4:
                            min = -100; max = 0; break;

                        case 2:
                            min = 0; max = 85; break;

                        default:
                            min = -75; max = 75; break;
                    }

                    angleDifference = Mathf.DeltaAngle(hit.transform.eulerAngles.y, agent.transform.eulerAngles.y);

                    if ((angleDifference <= max && angleDifference >= min))
                    {
                        isStop = true;
                        break;
                    }
                }
            }

            if (GetIfItIsWaiting() != toggleStop)
            {
                if (GetIfItIsWaiting())
                {
                    stopStorage = agent.destination;
                    agent.enabled = false;
                    toggleStop = true;
                }
                else
                {
                    agent.enabled = true;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(stopStorage, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        agent.SetDestination(stopStorage);
                    }else
                    {
                        if (hiddenObject)
                        {
                            trafficmanager.SpwanHiddenCarNew();
                        }

                        for(int i = 0; i < transform.childCount; i++)
                        {
                            Destroy(transform.GetChild(i).gameObject);
                        }
                        Debug.LogWarning("Accept: that Posion dossen macth Nav");
                        Destroy(gameObject);

                    }
                    

                    toggleStop = false;
                }
            }
        }
    }

    public bool InsertGoals(List<Vector4> position)
    {
        if (position != null)
        {
            Queue<Vector4> nextGoals = new Queue<Vector4>();
            foreach (Vector4 pos in position)
            {
                nextGoals.Enqueue(pos);
            }
            goals = nextGoals;
            return true;
        }
        else
        {
            Debug.LogWarning("Null list exception");
            return false;
        }
    }

    private void CorrectPosition(Vector3 position)
    {
        float xDistance = GetDistance(transform.position.x, position.x);
        float zDistance = GetDistance(transform.position.z, position.z);

        if (xDistance < zDistance)
        {
            goals.Enqueue(new Vector3(position.x, transform.position.y, agent.transform.position.z));
        }
        else
        {
            goals.Enqueue(new Vector3(transform.position.x, agent.transform.position.y, position.z));
        }
        goals.Enqueue(position);
    }

    private float GetDistance(float x1, float x2)
    {
        float dist = x2 - x1;
        return dist >= 0 ? dist : -dist;
    }

    public int GetPriority()
    {
        return priority;
    }

    public float GetRotation()
    {
        return transform.eulerAngles.y;
    }

    public bool GetIfItIsWaiting()
    {
        return isWait || isStop;
    }

    public void StopCar()
    {
        isWait = true;
    }

    public bool GoCar()
    {
        bool isNull = InsertGoals(lastPathNote.GetGoal(priority));
        isWait = false;
        isInIntersection = true;

        if (ignoreCollision)
        {
            agent.enabled = true;
        }

        return isNull;
    }
}
