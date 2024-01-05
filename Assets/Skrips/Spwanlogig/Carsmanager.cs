using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Spwanmanger", menuName = "Manager/Spwanmanger", order = 1)]
public class Carsmanager : MonoBehaviour
{
    

    [SerializeField]
    public CarRealtions HiddenObjekt;
    [SerializeField]
    public Dificulty dificultyObjekt;
    [SerializeField]
    GameObject car;
    [SerializeField]
    GameObject repesentativ;



    List<GameObject> spwarnpoins;
    List<GameObject> turns;
    List<GameObject> carlist;
    List<GameObject> todelaet;
    public int tospwan = 0;
    public int totalspawn = 0;

    [SerializeField]
    GameObject hiddenobjekt;
    [SerializeField]
    Displaycar cardisplay;

    // Start is called before the first frame update
    void Start()
    {
        spwarnpoins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Carspwarner"));
        turns = new List<GameObject>(GameObject.FindGameObjectsWithTag("turn"));

        todelaet = new List<GameObject>();
        carlist = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
       totalspawn = transform.childCount;
       checkChilden();
        if (transform.childCount < tospwan)
        {
            for (int sp = tospwan; transform.childCount < tospwan && sp > 0; sp--) 
            { 
                Spwancar(); 
            }
            tospwan = transform.childCount;
        }
    }
    [Button("Spwancar")]
    void Spwancar(bool isHiddenOBJ = false)
    {
        Transform getposion =null;
        GameObject VisalCartospwan = null;
        int random = Random.Range(0, spwarnpoins.Count-1);
        if (isHiddenOBJ) 
        {
            VisalCartospwan = HiddenObjekt.hiddenobjekt; 
        }
        else
        {
             VisalCartospwan = HiddenObjekt.getrendom(dificultyObjekt.curve);
        }
        for (int i = 0; getposion == null&&i<10;i++)
        {
            
            try{ getposion = spwarnpoins[(random+i)%spwarnpoins.Count].GetComponent<SpwanObjects>().spwancar(); }
            catch(System.Exception ex) { Debug.Log(ex); };



        }
        if(getposion != null)
        {
            GameObject Barincar = Instantiate(car, getposion.position, getposion.rotation);
            GameObject Visualcar = Instantiate(VisalCartospwan);
            Barincar.transform.parent= transform;
            Visualcar.transform.parent = Barincar.transform;
            Visualcar.transform.eulerAngles = Barincar.transform.eulerAngles;
            Visualcar.transform.localPosition = Vector3.zero;

            if (isHiddenOBJ && Barincar.TryGetComponent<NaveNextGoal>(out NaveNextGoal set))
            {
                set.Hiddenobjek = true;
                hiddenobjekt = Barincar;
                cardisplay.changeCar(Visualcar);
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
    public void creatfield(CarRealtions nextHIDOBJ, Dificulty nextdificulty,int maxcar)
    {
        HiddenObjekt = nextHIDOBJ;
        dificultyObjekt = nextdificulty;
        tospwan = maxcar;

        destroyallcars();
        foreach (GameObject obj in spwarnpoins)
        {
            if(obj.TryGetComponent<SpwanObjects>(out SpwanObjects sp))
            {
                sp.blocked = false;
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
