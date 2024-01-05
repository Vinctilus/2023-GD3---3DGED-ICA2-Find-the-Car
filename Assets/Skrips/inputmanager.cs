using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class inputmanager : MonoBehaviour
{
    [SerializeField]
    GameObject camera;

    [SerializeField]
    gameloic Gamemanager;

    [SerializeField]
    public AnimationCurve SpeedCurve;

    [SerializeField]
    float Keydowntime = 0;

    [SerializeField]
    public Vector3 ofset;
    [SerializeField]
    public float mouswheelseiblity = 10;
    [SerializeField]
    float gloablspeed = 5;
    [SerializeField]
    float deltatime = 0;

    //touch 
    [SerializeField]
    Vector3 touchdown;
    [SerializeField]
    float pinchcoretion = 0.01f;
    [SerializeField]
    float movecoretion = 10f;
    [SerializeField]
    float deathzone = 10f;
    [SerializeField]
    bool deathzoneaktive = false;

    //Boaders
    [SerializeField]
    Vector3 minBoader;
    [SerializeField]
    Vector3 maxBoader;


    [SerializeField]
    SphereManager spheremanager;
    [SerializeField]
    float benitigrange = 0.1f;
    [SerializeField]
    float benitingoffest = 0.05f;
    

    //Selection
    [SerializeField]
    LayerMask layerMaskVisualCars;
    [SerializeField]
    float Clickisdown =0;
    [SerializeField]
    float maxdilay = 5f;
    GameObject HitGameobjekt;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        deltatime = gloablspeed + Time.deltaTime;


        Vector3 keyinput = keyContols();
        Vector3 touchinput = touchContols();
        Vector3 nexpositon = camera.transform.position + keyinput + touchinput;
        camera.transform.position = VectorLimit(nexpositon);

        spheremanager.bendingAmount = Mathf.Max(0, (benitigrange * 1/ (maxBoader.y - minBoader.y) * (camera.transform.position.y - minBoader.y))- benitingoffest);

        //seltion of carControllerPrefab
        selection();

    }

    Vector3 keyContols()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Keydowntime < SpeedCurve.length)
            {
                Keydowntime += Time.deltaTime;
            }
            if (Keydowntime > SpeedCurve.length)
            {
                Keydowntime = SpeedCurve.length;
            }
        }
        else
        {
            Keydowntime = 0;
        }


        return new Vector3((Input.GetAxis("Horizontal") * SpeedCurve.Evaluate(Keydowntime)) * deltatime, Input.GetAxis("Mouse ScrollWheel") * mouswheelseiblity * deltatime, (Input.GetAxis("Vertical") * SpeedCurve.Evaluate(Keydowntime) * deltatime));
    }



    Vector3 touchContols()
    {
        // the tochcontrol part is creat with the tutorial https://www.youtube.com/watch?v=K_aAnBn5khA
        Vector3 direction = Vector3.zero;
        Vector3 zoom = Vector3.zero;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        if (Input.GetMouseButtonDown(0))
        {
            touchdown = mousePosition;
        }
        if (Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            Vector2 PrePosOne = touchOne.position - touchOne.deltaPosition;
            Vector2 PrePosTow = touchTwo.position - touchTwo.deltaPosition;

            float preMagintude = (PrePosOne - PrePosTow).magnitude;
            float currentMagintude = (touchOne.position - touchTwo.position).magnitude;
            zoom = new Vector3(0, currentMagintude - preMagintude, 0)* pinchcoretion;
        }
        else if (Input.GetMouseButton(0))
        {

            float slowbydistac = 1+1 / (maxBoader.y-minBoader.y) * (camera.transform.position.y - minBoader.y);
            
            if (Mathf.Abs(Vector3.Distance(touchdown, mousePosition))>= deathzone || deathzoneaktive) { 
                      
            direction = (touchdown - mousePosition) * movecoretion * slowbydistac;
                deathzoneaktive = true;


            }

            touchdown = mousePosition;
        }
        else
        {
            if (deathzoneaktive) {deathzoneaktive = false;}
        }


        return direction + zoom;
    }

    void keeplimit(ref float var, float min, float max)
    {
        if (var < min)
        {
            if (var < min - 3)
                var = min - 3;
            else
                var = var + 0.2f*deltatime;
        }
        if (var > max)
        {
            if (var > max + 3)
                var = max + 3;
            else
                var = var - 0.2f*deltatime;
        }
    }

    void roundlimt(ref float var, float min, float max)
    {
        if (var < min)
        {
            var += max-min;
        }
        if (var > max)
        {
            var -= max - min;
        }
    }

    Vector3 VectorLimit(Vector3 v)
    {
        keeplimit(ref v.x, minBoader.x, maxBoader.x);
        keeplimit(ref v.y,minBoader.y,maxBoader.y);
        keeplimit(ref v.z, minBoader.z, maxBoader.z);
        return v;
    }


    void selection()
    {
        if (!deathzoneaktive)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Clickisdown = 0;
                HitGameobjekt = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,1000f, layerMaskVisualCars))
                {

                    HitGameobjekt = hit.collider.gameObject;
                }
            }
            bool isdown = false;
            if (Input.GetMouseButton(0) && Clickisdown< maxdilay)
            {
                Clickisdown += Time.deltaTime;
                isdown = true;
            }

            if (!isdown && HitGameobjekt != null)
            {
                
                if (Clickisdown < maxdilay )
                {

                    //Debug.Log(HitGameobjekt.transform.parent.gameObject.GetComponent<CarController>().hiddenObject);
                    Gamemanager.chechifhasscord(HitGameobjekt.transform.parent.gameObject.GetComponent<CarController>().hiddenObject);
                    HitGameobjekt = null;
                }
            }
        }
        else { Clickisdown = 0; HitGameobjekt = null; }
    }
}
