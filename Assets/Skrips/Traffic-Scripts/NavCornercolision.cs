using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class NavCornercolision : MonoBehaviour
{
    [SerializeField]
    bool _dontneedinTurn = false;
    [SerializeField]
    bool _straight = false;
    [SerializeField]
    bool _right = false;
    [SerializeField]
    bool _left = false;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    LayerMask layerMaskCars;
    [SerializeField]
    Vector4 _straightV;
    [SerializeField]
    Vector4 _rightV;
    [SerializeField]
    Vector4 _leftV;

    [SerializeField]
    public GameObject cubePrefab;


    //0:left,1:straight,2:right
    [SerializeField]
    private Dictionary<int,List<Vector4>> posibile_directios = new Dictionary<int,List<Vector4>>();


    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;
        
        Vector3 offset = kVd(transform.position - parentObject.transform.position);
        float stoppline = 0;
        float center = 0;


        if (offset.x > offset.z)
        {
            center = offset.z;
            stoppline = offset.x;
        }
        else
        {

            center = offset.x;
            stoppline = offset.z;
        }


        if (_straight)
        {
            posibile_directios.Add(3,new List<Vector4> { prio(transform.position + Filter(offset, transform.forward*1f),3) });
            posibile_directios[3].Add(prio(transform.position + Filter(offset, transform.forward * 2f), 3));

            _straightV = serachforgoal(transform.forward, posibile_directios[3][1],3);
            posibile_directios[3].Add(prio(transform.forward, -1));

        }
        if (_right)
        {

            posibile_directios.Add(4,new List<Vector4> { prio(transform.position + Filter(offset, transform.forward* ((stoppline - center)/ stoppline)),4) });
            posibile_directios[4].Add(prio(transform.position + Filter(offset, transform.forward * ((stoppline - center) / stoppline) + transform.right * ((stoppline - center) / center)), 4));

            _rightV = serachforgoal(transform.right, posibile_directios[4][1],4);
            posibile_directios[4].Add(prio(transform.right, -1));

        }
        if (_left) {
            posibile_directios.Add(2,new List<Vector4> { prio(transform.position + Filter(offset, transform.forward*1.2f - transform.right*0.8f), 2) });
            posibile_directios[2].Add(prio(transform.position + Filter(offset, transform.forward * ((stoppline + center) / stoppline) - transform.right * ((stoppline + center) / center)), 2));

            _leftV = serachforgoal(-transform.right, posibile_directios[2][1],2);
            posibile_directios[2].Add(prio(-transform.right, -1));

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (posibile_directios.Count == 0) { Start(); }
    }

    private Vector4 serachforgoal(Vector3 direction, Vector3 position,int x)
    {
        float rayLength = 1000f;
        Ray ray = new Ray(position - direction*10f, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            Vector4 temp = prio(hit.transform.position+direction*5, 0);


            posibile_directios[x].Add(temp);
            return temp;
        }
        else { return Vector4.zero; }


    }


    public List<Vector4> getgoal(int a)
    {
        if (!IsAKey(a))
        {
            a = rendomKey(Random.Range(0, posibile_directios.Count()));
        }

        if (IsAKey(a))
        {
            List<Vector4> positions = posibile_directios[a];

            if (!_dontneedinTurn && posibile_directios.Count > 1)
            {
                Vector3 direction = positions[3];
                Vector3 from = positions[1];
                Ray ray = new Ray(from - direction * 10f, direction);
                RaycastHit[] hits = Physics.SphereCastAll(ray, 2f, 10f, layerMaskCars);

                bool gothit = false;
                int i = 0;
                foreach (var hit in hits)
                {
                    i++;

                    if (hit.transform.gameObject.TryGetComponent<NaveNextGoal>(out NaveNextGoal setter))
                    {
                        if (!setter.ignorcolision && setter.getifitisWating())
                        {

                            gothit = true;

                        }
                    }

                }

                if (gothit)
                {
                    List<Vector4> otherpositions = getother(posibile_directios.Keys.ToList());
                    if(otherpositions != null)
                    {
                        positions = otherpositions;

                    }



                }

                
            }

            return new List<Vector4>() { positions[0], positions[1], positions[2] };
        }
        Debug.LogWarning("Wth wie kann das leer sein???");
        return null;

    }

    public List<Vector4> getother(ICollection<int> keys)
    {
        foreach(var key in keys) {
            List<Vector4> positions = posibile_directios[key];


            Vector3 direction = positions[3];
            Vector3 from = positions[1];
            Ray ray = new Ray(from - direction * 10f, direction);

            

            RaycastHit[] hits = Physics.SphereCastAll(ray, 2f, 10f, layerMaskCars);
            bool gothit = false;
            int i = 0;
            foreach (var hit in hits)
            {
                i++;
                
                if (hit.transform.gameObject.TryGetComponent<NaveNextGoal>(out NaveNextGoal setter))
                {
                    if (!setter.ignorcolision && setter.getifitisWating())
                    {
                        gothit = true;
                        break;
                    }
                }

            }
            if (!gothit) {return positions; }



        }


        return null;
    }


    private void OnTriggerEnter(Collider other)
    {

        other.gameObject.GetComponent<NaveNextGoal>().Lastcorner = this;
        if (other.gameObject.CompareTag("Car"))
        {
            NaveNextGoal setter = other.gameObject.GetComponent<NaveNextGoal>();
            if (setter != null)
            { 
                if (_dontneedinTurn)
                {
                    setter.insertgoale(getgoal(0));
                }
                else
                {
                    setter.prio = rendomKey(Random.Range(0, posibile_directios.Count())); ;
                }

            }
        }
    }
    //Is filtering the Vector
    private Vector3 Filter(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }


    //take the dirctition of the Vector
    private Vector4 prio(Vector3 v1,float a)
    {
        return new Vector4(v1.x,v1.y, v1.z,a);
    }
    private Vector3 kVd(Vector3 v1)
    {
        return new Vector3(kd(v1.x), kd(v1.y), kd(v1.z));
    }
    //do negativ float to positive number
    private float kd(float v)
    {
        return v < 0 ? -v : v;
    }


    int rendomKey(int a)
    {
       
        List<int> keyCollection = new List<int>();
        foreach (int key in posibile_directios.Keys) { keyCollection.Add(key); }

        int randomIndex = 0;
        if (keyCollection.Count > 1)
        {
            randomIndex = a % (keyCollection.Count - 1);
        }
        return keyCollection[randomIndex];
    }


    bool IsAKey(int a)
    {
        return posibile_directios.ContainsKey(a);
    }

}
