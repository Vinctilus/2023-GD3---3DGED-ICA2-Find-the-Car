using DG.Tweening.Core;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class IntersectionManager : MonoBehaviour
{
    [Header("Debug")]
    public SortedDictionary<int, Queue<GameObject>> carQueues = new SortedDictionary<int, Queue<GameObject>>();
    public List<GameObject> carInsideList = new List<GameObject>();
    [SerializeField]
    public int carCountInside = 0;
    [SerializeField]
    int lastPriority = -1;
    [SerializeField]
    float lastRotation = 0f;
    [SerializeField]
    float simplifyRotation4 = 0;
    void Start()
    {
        lastPriority = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<CarController>().lastIntersection = this;
        EnqueueCar(other.gameObject);
    }

    public void EnqueueCar(GameObject other)
    {
        CarController setter = other.GetComponent<CarController>();
        if (setter != null)
        {
            int priority = setter.GetPriority();
            if (!carQueues.ContainsKey(priority))
            {
                carQueues[priority] = new Queue<GameObject>();
            }

            carQueues[priority].Enqueue(other);
            setter.StopCar();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        carInsideList.Remove(other.gameObject);
        if (other.gameObject.CompareTag("Car"))
        {
            carCountInside = Mathf.Max(0, carCountInside - 1);

            if (other.gameObject.TryGetComponent<CarController>(out CarController b))
            {
                b.isInIntersection = false;
            }

            if (carCountInside == 0)
            {
                lastPriority = -1;
            }
        }
    }

    private void Update()
    {
        simplifyRotation4 = ShortRotation(lastRotation);

        if (carQueues.Count > 0)
        {
            int i = 0;
            foreach (int key in new List<int>(carQueues.Keys))
            {
                i++;

                if (carQueues[key].Count == 0)
                {
                    carQueues.Remove(key);
                }
            }
        }

        if (carQueues.Count > 0)
        {
            if (lastPriority == -1 && carCountInside == 0)
            {
                GameObject ob = carQueues[carQueues.Keys.Last()].Dequeue();
                ActivateCar(ob);
            }
            else if (lastPriority == 4)
            {
                if (carQueues.ContainsKey(lastPriority) && carQueues[lastPriority].Count > 0)
                {
                    ActivateCar(carQueues[lastPriority].Dequeue());
                }
                else if (carQueues.ContainsKey(3))
                {
                    CheckQueue(3, 4, -1, true);
                }
                else
                {
                    CheckQueue(2, 4, 1, true);
                }
            }
            else if (lastPriority == 3)
            {
                CheckQueue(4, 2, 0);
                CheckQueue(3, 2, 0);
            }
            else if (lastPriority == 2)
            {
                if (carQueues.ContainsKey(lastPriority) && carQueues[lastPriority].Count > 0)
                {
                    CheckQueue(2, 4, 0);
                }
                else
                {
                    CheckQueue(4, 4, 1);
                }
            }
        }
    }

    private void CheckQueue(int priority, int limit, int offset, bool allowChange = false)
    {
        float x = lastRotation;
        int y = lastPriority;
        Queue<GameObject> cannotPass = new Queue<GameObject>();
        if (carQueues.ContainsKey(priority))
        {
            foreach (GameObject car in carQueues[priority])
            {
                if (car != null)
                {
                    int a = (limit + (ShortRotation(car.transform.eulerAngles.y))) % limit;
                    int b = (limit + (ShortRotation(lastRotation) + offset)) % limit;

                    if (a == b)
                    {
                        ActivateCar(car);
                    }
                    else
                    {
                        cannotPass.Enqueue(car);
                    }
                }
            }
            carQueues[priority] = cannotPass;
        }
        if (!allowChange)
        {
            lastRotation = x;
            lastPriority = y;
        }
    }

    private void ActivateCar(GameObject nextCar)
    {
        if (nextCar.TryGetComponent<CarController>(out CarController setter))
        {
            if (setter.GoCar())
            {
                carInsideList.Add(nextCar);
                carCountInside = carCountInside + 1;
                lastPriority = setter.GetPriority();
                lastRotation = setter.GetRotation();
            }
            else
            {
                EnqueueCar(nextCar);
            }
        }
    }

    public void ByFullIntersection()
    {
        lastPriority = -1;

        foreach (GameObject car in carInsideList)
        {
            if (car != null)
            {
                car.SetActive(false);
                if (!car.GetComponent<CarController>().hiddenObject)
                {
                    for (int i = 0; i < car.transform.childCount; i++)
                    {
                        Destroy(car.transform.GetChild(i).gameObject);
                    }
                    Destroy(car);

                }
            }
        }
        carCountInside = 0;
        carInsideList = new List<GameObject>();
    }

    private int ShortRotation(float a)
    {
        return (int)Mathf.Round(a / 90);
    }
}