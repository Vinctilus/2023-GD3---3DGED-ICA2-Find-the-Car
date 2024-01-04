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


    List<GameObject> spwarnpoins;
    List<GameObject> todelaet;
    public int tospwan = 0;
    



    // Start is called before the first frame update
    void Start()
    {
        spwarnpoins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Carspwarner"));
        todelaet = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Button("Spwancar")]
    void Spwancar()
    {
        Transform transform = null;
        int random = Random.Range(0, spwarnpoins.Count-1);
        GameObject VisalCartospwan = HiddenObjekt.getrendom(dificultyObjekt.curve);
        Debug.Log(spwarnpoins.Count);
        for (int i = 0; transform == null&&i<10;i++)
        {
            transform = spwarnpoins[random].GetComponent<SpwanObjects>().spwancar();
        }
        if(transform != null)
        {
            GameObject Barincar = Instantiate(car, transform.position,transform.rotation);
            GameObject Visualcar = Instantiate(VisalCartospwan);
            Visualcar.transform.parent = Barincar.transform;
            Visualcar.transform.eulerAngles = Barincar.transform.eulerAngles;
            Visualcar.transform.localPosition = Vector3.zero;
        }
    }
}
