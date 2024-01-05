using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrafficManager : MonoBehaviour
{

    [Header("Get from GameManager")]
    [SerializeField]
    public CarRelationConnections carRelationObject;
    [SerializeField]
    public DifficultyCurve difficultyObject;

    [Header("Prefabs")]
    [SerializeField]
    GameObject carControllerPrefab;

    [Header("Display Car")]
    [SerializeField]
    DisplayCar cardisplay;


    [Header("Systems")]
    public List<GameObject> spwarnpoinsList;
    public List<GameObject> intersectionsList;
    public int maxCarsToSpawn = 0;
    public int totalCarsCount = 0;

    [Header("Actual Object to Search")]
    [SerializeField]
    GameObject hiddenObject;
    

    // Start is called before the first frame update
    void Start()
    {
        spwarnpoinsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Carspwarner"));
        intersectionsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("turn"));
    }

    // Update is called once per frame
    void Update()
    {
       totalCarsCount = transform.childCount;
       checkChilden();
        if (transform.childCount < maxCarsToSpawn)
        {
            for (int sp = maxCarsToSpawn; transform.childCount < maxCarsToSpawn && sp > 0; sp--) 
            { 
                Spwancar(); 
            }
            maxCarsToSpawn = transform.childCount;
        }
    }
    [Button("Spwancar")]
    void Spwancar(bool isHiddenOBJ = false)
    {
        Transform getposion =null;
        GameObject VisalCartospwan = null;
        int random = Random.Range(0, spwarnpoinsList.Count-1);
        if (isHiddenOBJ) 
        {
            VisalCartospwan = carRelationObject.hiddenobjekt; 
        }
        else
        {
             VisalCartospwan = carRelationObject.GetRandom(difficultyObject.curve);
        }
        for (int i = 0; getposion == null&&i<10;i++)
        {
            
            try{ getposion = spwarnpoinsList[(random+i)%spwarnpoinsList.Count].GetComponent<CheckCarPlacement>().spwancar(); }
            catch(System.Exception ex) { Debug.Log(ex); };



        }
        if(getposion != null)
        {
            GameObject Barincar = Instantiate(carControllerPrefab, getposion.position, getposion.rotation);
            GameObject Visualcar = Instantiate(VisalCartospwan);
            Barincar.transform.parent= transform;
            Visualcar.transform.parent = Barincar.transform;
            Visualcar.transform.eulerAngles = Barincar.transform.eulerAngles;
            Visualcar.transform.localPosition = Vector3.zero;

            if (isHiddenOBJ && Barincar.TryGetComponent<CarController>(out CarController set))
            {
                set.hiddenObject = true;
                hiddenObject = Barincar;
                cardisplay.ChangeCar(Visualcar);
            }

                Barincar.SetActive(true);



        }
        
    }

    [Button("destroyallcars")]
    void destroyallcars()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject obj = transform.GetChild(i).gameObject;


                for (int b = 0; b < obj.transform.childCount; b++)
                {
                    Destroy(obj.transform.GetChild(b).gameObject);
                }
                Destroy(obj);
            
        }
    }


    void checkChilden()
    {
       for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject obj = transform.GetChild(i).gameObject;
        
            if (!obj.activeSelf)
            {
                for (int b = 0; b < obj.transform.childCount; b++)
                {
                    Destroy(obj.transform.GetChild(b).gameObject);
                }
                Destroy(obj);
            }
        }
    }

    [Button("Test game mode")]
    public void creatfield(CarRelationConnections nextHIDOBJ, DifficultyCurve nextdificulty,int maxcar)
    {
        carRelationObject = nextHIDOBJ;
        difficultyObject = nextdificulty;
        maxCarsToSpawn = maxcar;
        foreach (var obj in intersectionsList)
        {
            if(obj.TryGetComponent<IntersectionManager>(out IntersectionManager intersection))
            {
                intersection.BySpwanNew();
            }
        }
        destroyallcars();
        foreach (GameObject obj in spwarnpoinsList)
        {
            if(obj.TryGetComponent<CheckCarPlacement>(out CheckCarPlacement sp))
            {
                sp.isBlocked = false;
            }
        }

        Spwancar(true);

    }


    public void carssetactive(bool aktiv)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            obj.SetActive(aktiv);
        }
    }
}
