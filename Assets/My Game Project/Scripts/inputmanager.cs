using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class InputManager : MonoBehaviour
{
    [Header("Get Objects")]
    [SerializeField]
    GameObject cameraObject;
    [SerializeField]
    SphereManager sphereManager;

    [Header("Get Layer")]
    [SerializeField]
    LayerMask layerMaskVisualCars;

    [Header("GameManager")]
    [SerializeField]
    GameLogic gamemanager;

    [Header("Keyboard Controls")]
    [SerializeField]
    public AnimationCurve speedCurve;
    [SerializeField]
    float keyDownTime = 0;
    [SerializeField]
    public Vector3 offset;
    [SerializeField]
    public float mouseWheelSensitivity = 10;
    [SerializeField]
    float globalSpeed = 0.9f;
    [SerializeField]
    float deltaTime = 0;

    [Header("Toch/Mouse Controls")]
    [SerializeField]
    Vector3 touchDown;
    [SerializeField]
    float pinchCorrection = 0.5f;
    [SerializeField]
    float moveCorrection = 0.1136364f;
    [SerializeField]
    float deathZone = 3f;
    [SerializeField]
    bool deathZoneOff = false;

    [Header("Boaders")]
    [SerializeField]
    Vector3 minBorder = new Vector3(30f, 50f, 30f);
    [SerializeField]
    Vector3 maxBorder = new Vector3(300f, 90f, 300f);



    [SerializeField]
    float benitigRange = 0.004f;
    [SerializeField]
    float benitingOffset = 0f;


    [Header("Selection system")]
    [SerializeField]
    float clickIsDown = 0;
    [SerializeField]
    float maxDelay = 2.5f;
    [Header("Selection system Debug")]
    [SerializeField]
    GameObject hitGameObject;

    void Start()
    {
        enabled = false;
    }

    void Update()
    {
        deltaTime = globalSpeed + Time.deltaTime;

        Vector3 keyInput = KeyControls();
        Vector3 touchInput = TouchControls();
        Vector3 nextPosition = cameraObject.transform.position + keyInput + touchInput;
        cameraObject.transform.position = VectorLimit(nextPosition);

        sphereManager.bendingAmount = Mathf.Max(0, (benitigRange * 1 / (maxBorder.y - minBorder.y) * (cameraObject.transform.position.y - minBorder.y)) - benitingOffset);

        // Selection of CarControllerPrefab
        Selection();
    }

    Vector3 KeyControls()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (keyDownTime < speedCurve.length)
            {
                keyDownTime += Time.deltaTime;
            }
            if (keyDownTime > speedCurve.length)
            {
                keyDownTime = speedCurve.length;
            }
        }
        else
        {
            keyDownTime = 0;
        }

        return new Vector3((Input.GetAxis("Horizontal") * speedCurve.Evaluate(keyDownTime)) * deltaTime, Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity * deltaTime, (Input.GetAxis("Vertical") * speedCurve.Evaluate(keyDownTime) * deltaTime));
    }

    Vector3 TouchControls()
    {
        // The touch control part was created with the tutorial https://www.youtube.com/watch?v=K_aAnBn5khA
        Vector3 direction = Vector3.zero;
        Vector3 zoom = Vector3.zero;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);

        if (Input.GetMouseButtonDown(0))
        {
            touchDown = mousePosition;
        }

        if (Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            Vector2 prePosOne = touchOne.position - touchOne.deltaPosition;
            Vector2 prePosTwo = touchTwo.position - touchTwo.deltaPosition;

            float preMagnitude = (prePosOne - prePosTwo).magnitude;
            float currentMagnitude = (touchOne.position - touchTwo.position).magnitude;
            zoom = new Vector3(0, currentMagnitude - preMagnitude, 0) * pinchCorrection;
        }
        else if (Input.GetMouseButton(0))
        {
            float slowByDistance = 1 + 1 / (maxBorder.y - minBorder.y) * (cameraObject.transform.position.y - minBorder.y);

            if (Mathf.Abs(Vector3.Distance(touchDown, mousePosition)) >= deathZone || deathZoneOff)
            {
                direction = (touchDown - mousePosition) * moveCorrection * slowByDistance;
                deathZoneOff = true;
            }

            touchDown = mousePosition;
        }
        else
        {
            if (deathZoneOff) { deathZoneOff = false; }
        }

        return direction + zoom;
    }

    void KeepLimit(ref float var, float min, float max)
    {
        if (var < min)
        {
            if (var < min - 3)
                var = min - 3;
            else
                var = var + 0.2f * deltaTime;
        }
        if (var > max)
        {
            if (var > max + 3)
                var = max + 3;
            else
                var = var - 0.2f * deltaTime;
        }
    }


    void RoundLimit(ref float var, float min, float max)
    {
        if (var < min)
        {
            var += max - min;
        }
        if (var > max)
        {
            var -= max - min;
        }
    }

    Vector3 VectorLimit(Vector3 v)
    {
        KeepLimit(ref v.x, minBorder.x, maxBorder.x);
        KeepLimit(ref v.y, minBorder.y, maxBorder.y);
        KeepLimit(ref v.z, minBorder.z, maxBorder.z);
        return v;
    }

    void Selection()
    {
        if (!deathZoneOff)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickIsDown = 0;
                hitGameObject = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, layerMaskVisualCars))
                {
                    hitGameObject = hit.collider.gameObject;
                }
            }

            bool isDown = false;
            if (Input.GetMouseButton(0) && clickIsDown < maxDelay)
            {
                clickIsDown += Time.deltaTime;
                isDown = true;
            }

            if (!isDown && hitGameObject != null)
            {
                if (clickIsDown < maxDelay)
                {
                    gamemanager.HitOnCar(hitGameObject.transform.parent.gameObject.GetComponent<CarController>().hiddenObject);
                    hitGameObject = null;
                }
            }
        }
        else
        {
            clickIsDown = 0;
            hitGameObject = null;
        }
    }
}

