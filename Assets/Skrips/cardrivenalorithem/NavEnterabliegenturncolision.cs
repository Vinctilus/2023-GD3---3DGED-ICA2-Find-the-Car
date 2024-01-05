using DG.Tweening.Core;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class NavEnterTorunColision : MonoBehaviour
{
    [SerializeField]
    public SortedDictionary<int, Queue<GameObject>> CarQueues = new SortedDictionary<int, Queue<GameObject>>();
    public List<GameObject> CarInsideList = new List<GameObject>();
    [SerializeField]
    public int carCountInside = 0;
    [SerializeField]
    int lastPriority = -1;
    [SerializeField]
    float lastRotation = 0f;
    [SerializeField]
    float shortrotaoin = 0;
    [SerializeField]
    bool debugit=false;

    void Start()
    {
        lastPriority = -1;
    }

        private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<NaveNextGoal>().Lasturn = this;
        enqueueCar(other.gameObject);
    }

    public void enqueueCar(GameObject other)
    {
            NaveNextGoal setter = other.GetComponent<NaveNextGoal>();
            if (setter != null)
            {
                int priority = setter.GetPriority();
                if (!CarQueues.ContainsKey(priority))
                {
                    CarQueues[priority] = new Queue<GameObject>();
                }

                CarQueues[priority].Enqueue(other);
                setter.stopp_car();
            }
    }
    private void OnTriggerExit(Collider other)
    {
        CarInsideList.Remove(other.gameObject);
        if (other.gameObject.CompareTag("Car"))
        {
            carCountInside = Mathf.Max(0, carCountInside - 1);

            if (other.gameObject.TryGetComponent<NaveNextGoal>(out NaveNextGoal b))
            { 
                b.insection = false;
            }
            

            if (carCountInside == 0)
            {
                lastPriority = -1;
            }
        }
       
    }

    private void Update()
    {
        shortrotaoin = schortrotaion(lastRotation);

        if (CarQueues.Count > 0)
        {
            int i=0;
            foreach (int key in new List<int>(CarQueues.Keys))
            {
                i++;

            if (CarQueues[key].Count == 0)
                {
                    CarQueues.Remove(key);
                }

            }
        }

        if (CarQueues.Count > 0) 
        { 

            if (lastPriority == -1 && carCountInside ==0)
            {
                if (debugit)
                {
                    foreach(int key in CarQueues.Keys)
                    {
                        Debug.Log(key);
                    }
                }
                GameObject ob = CarQueues[CarQueues.Keys.Last()].Dequeue();
                avtivate_car(ob);


            }
            else if (lastPriority == 4)
            {
                if (CarQueues.ContainsKey(lastPriority) && CarQueues[lastPriority].Count > 0)
                {
                    avtivate_car(CarQueues[lastPriority].Dequeue());
                }
                else if(CarQueues.ContainsKey(3))
                {

                    CheckinQueue(3, 4,-1, true);
                    



                }
                else
                {
                    CheckinQueue(2, 4, 1, true);
                    
                }
            }
            else if (lastPriority == 3)
            {
                CheckinQueue(4, 2,0);
                CheckinQueue(3, 2,0);
 
            }

            else if (lastPriority == 2)
            {

                if (CarQueues.ContainsKey(lastPriority) && CarQueues[lastPriority].Count > 0)

                {

                    CheckinQueue(2, 4,0);
                }
                else
                {

                    CheckinQueue(4, 4, 1);


                }
            }
        }

        

    }

    private void CheckinQueue(int priority, int limit,int offset,bool allowchange =false)
    {


        float x = lastRotation;
        int y = lastPriority;
        Queue<GameObject> cannotpass = new Queue<GameObject>();
        if (CarQueues.ContainsKey(priority))
        {
            foreach (GameObject car in CarQueues[priority])
            {
                if (car != null)
                {
                    int a = (limit + (schortrotaion(car.transform.eulerAngles.y))) % limit;
                    int b = (limit + (schortrotaion(lastRotation) + offset)) % limit;

                    if ( a == b)
                    {
                        avtivate_car(car);
                    }
                    else
                    {
                        cannotpass.Enqueue(car);
                    }
                }
            }
            CarQueues[priority] = cannotpass;
        }
        if (!allowchange)
        {
            lastRotation = x;
            lastPriority = y;
        }

    }




    private void avtivate_car(GameObject next_car)
        {

        if (next_car.TryGetComponent<NaveNextGoal>(out NaveNextGoal setter))

            if (setter.GoCar())
            {
                CarInsideList.Add(next_car);
                carCountInside = carCountInside + 1;
                lastPriority = setter.GetPriority();
                lastRotation = setter.GetRotation();
            }
            else
            {
                enqueueCar(next_car);
            }
                

    }

        

    public void byfullsaction()
    {

        if(debugit) { Debug.Log("run 5 add"); }
        lastPriority = -1;

        foreach (GameObject car in CarInsideList) {
            if (car != null)
            {
                car.SetActive(false);
                if (!car.GetComponent<NaveNextGoal>().Hiddenobjek)
                {
                    for (int i = 0; i < car.transform.childCount; i++)
                    {
                        Destroy(car.transform.GetChild(i).gameObject);
                    }
                    Destroy(car);
                   
                    #region todo
                    //send spwarn logig spwan new car
                    //!
                    //!
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    #endregion
                }
            }

        }
        carCountInside = 0;
        CarInsideList = new List<GameObject>();

    }

    private int schortrotaion(float a)
    {
        return (int)Mathf.Round(a/90);
    }
 
}
