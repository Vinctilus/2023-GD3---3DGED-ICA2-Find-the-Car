using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;
using System.Linq;

public class NaveNextGoal : MonoBehaviour
{
    [SerializeField]
    float Rotation4 = 0;
    [SerializeField]
    int Qlenth = 0;
    public Queue<Vector4> goals = new Queue<Vector4>();
    [SerializeField]

    NavMeshAgent Agent;
    [SerializeField]
    LayerMask layerMaskCorner;
    [SerializeField]
    LayerMask layerMaskCars;
    [SerializeField]
    LayerMask layerCarsoff;
    [SerializeField]
    float raycastCarslenth = 1;
    [SerializeField]
    private float raycastCarsSize = 4;
    [SerializeField]
    Vector3 stopstrage;
    [SerializeField]
    public bool isstopp = false;
    [SerializeField]
    public bool wait = false;
    [SerializeField]
    public bool ignorcolision = false;
    [SerializeField]
    public float waittime = 0;

    [SerializeField]
    bool toggelstopp = false;
    [SerializeField]
    float angleDifference;
    [SerializeField]
    public NavCornercolision Lastcorner;
    public NavEnterTorunColision Lasturn;
    [SerializeField]
    public bool Hiddenobjek = false;

    [SerializeField]
    bool debugit = false;

    [SerializeField]
    public int prio;
    [SerializeField]
    public bool insection;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();

        byemtyqueue();

    }

    private void byemtyqueue()
    {
        if (Lastcorner == null)
        {
            float rayLength = 1000f;
            Ray ray = new Ray(Agent.transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength, layerMaskCorner))
            {
                coreactPosition(hit.transform.position + transform.forward * 5);
            }
        }
        else
        {
            insertgoale(Lastcorner.getgoal(prio));
            goals.Dequeue();
            goals.Dequeue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(goals.Count == 0 &&(insection)) { insection = false; }
        if (goals.Count == 1 && (ignorcolision)) { ignorcolision = false; gameObject.layer = LayerMask.NameToLayer("Cars"); }


        if (getifitisWating() && insection)
        {
            waittime += Time.deltaTime;
            if (waittime >= 10) {
                
                Lasturn.byfullsaction();
                waittime = 0;
            
            }

        }
        else { waittime = 0; }

        if (goals.Count == 0)
        {
            byemtyqueue();
        }



        Rotation4 = Mathf.Round(transform.eulerAngles.y / 90);
        Qlenth = goals.Count;

        if (goals.Count == 0 && !wait)
        {
            byemtyqueue();
        }

        if (Agent.enabled && Vector3.Distance(Agent.transform.position, Agent.destination) <= 1.5 && goals.Count != 0)
        {

            Vector4 split = goals.Dequeue();
            stopstrage = new Vector3(split.x, split.y, split.z);
            Agent.destination = new Vector3(split.x, split.y, split.z);
            Agent.avoidancePriority = (int)split.w;
            prio = (int)split.w;

        }


        if (!ignorcolision)
        {

            Ray ray = new Ray(Agent.transform.position + transform.forward * (1f + (raycastCarsSize / 2)), transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(ray, raycastCarsSize, raycastCarslenth, layerMaskCars);

            isstopp = false;
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject != gameObject)
                {

                    float min = 0; float max = 0;
                    switch (prio)
                    {
                        case 4:
                            min = -100; max = 0; break;

                        case 2:
                            min = 0; max = 100; break;

                        default:
                            min = -75; max = 75; break;
                    }

                    angleDifference = Mathf.DeltaAngle(hit.transform.eulerAngles.y, Agent.transform.eulerAngles.y);

                    if ((angleDifference <= max && angleDifference >= min))
                    {
                        isstopp = true;
                        break;
                    }

                }
            }


            if (getifitisWating() != toggelstopp)
            {
                if (getifitisWating())
                {
                    stopstrage = Agent.destination;
                    Agent.enabled = false;
                    toggelstopp = true;
                }
                else
                {

                    Agent.enabled = true;
                    try{ Agent.destination = stopstrage; }
                    catch { }
                   
                    toggelstopp = false;
                }

            }

        }
        
        
    }



    public bool insertgoale(List<Vector4> position)
    {
        if (position != null)
        {
            Queue<Vector4> nextgoals = new Queue<Vector4>();
            foreach (Vector4 pos in position)
            {
                nextgoals.Enqueue(pos);
            }
            goals = nextgoals;
            return  true;
        }
        else { Debug.LogWarning("Null list acception"); return  false; }
        
    }

        private void coreactPosition(Vector3 position)
        {

            float xdistance = getdistace(transform.position.x, position.x);
            float zdistance = getdistace(transform.position.z, position.z);


            if (xdistance < zdistance)
            {
                goals.Enqueue(new Vector3(position.x, transform.position.y, Agent.transform.position.z));
            }
            else
            {
                goals.Enqueue(new Vector3(transform.position.x, Agent.transform.position.y, position.z));
            }
            goals.Enqueue(position);
        }
    

    private float getdistace(float x1, float x2)
    {
        float dist = x2 - x1;
        return dist >= 0 ? dist : -dist;
    }

    public int GetPriority()
    {
        return prio;
    }
    public float GetRotation()
    {
        return transform.eulerAngles.y;
    }


    public bool getifitisWating() { return wait || isstopp; }

    public void stopp_car()
    {
        wait = true;
        


    }
    public bool GoCar()
    {
        bool isnull = insertgoale(Lastcorner.getgoal(prio));
        wait = false;
        insection = true;

        if (ignorcolision)
        {
            Agent.enabled = true;
        }

        return isnull;

    }



}
