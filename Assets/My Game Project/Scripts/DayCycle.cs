using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Header("Directional Light")]
    public Light sunLight;
    public Light sunLight2;
    [Header("Settings")]
    [SerializeField]
    public bool timerIsActiv = false;
    [SerializeField]
    float timeForFullRotation = 10;
    [SerializeField]
    float minIntensity = 0.5f;
    [SerializeField]
    float maxIntensity =0.8f;
    [SerializeField]
    float dayNightSplit = 0.285398f;
    [SerializeField]
    Color soneset;
    [SerializeField]
    Color night;
    [Header("Debug")]
    [SerializeField]
    float timePast = 0;
    [SerializeField]
    List<GameObject> Lights;
    Material material;

    void Start()
    {
        Lights = new List<GameObject>(GameObject.FindGameObjectsWithTag("Light"));
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
        timePast = timeForFullRotation / 4;

        
    }

    void Update()
    {
        if (timerIsActiv)
        {
            timePast += Time.deltaTime;
            if (timePast > timeForFullRotation)
            {
                timePast -= timeForFullRotation;
            }

        }

        material.mainTextureOffset = new Vector2(0.5f+0.5f*Mathf.Sin(timePast / timeForFullRotation * Mathf.PI), 0);

        InterpolateLightColor();
        SwitchLight();

    }

    void InterpolateLightColor()
    {
        float t = Mathf.Sin(timePast / timeForFullRotation*Mathf.PI);
        Color color = Color.Lerp(night, Color.Lerp(soneset, Color.white, t), t);

        sunLight.color = color;
        sunLight2.color = color;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.InverseLerp(0.2f, 0.8f, t));
        sunLight.intensity = intensity;
        sunLight2.intensity = intensity;
    }
    void SwitchLight()
    {
        float t = Mathf.Sin(timePast / timeForFullRotation * Mathf.PI);
        if (t >= 0.0f && t < dayNightSplit)
        {
         
            SetLights(true);
        }
        else if (t > dayNightSplit && t <= 1.0f)
        {
            SetLights(false);
            
        }
    }

  
    void SetLights(bool toggle)
    { 
        foreach(GameObject obj in Lights)
        {
            if (obj.activeSelf != toggle)
            { 
                obj.SetActive(toggle);
            }
        }
    }

    public void GameStart()
    {
        timePast = timeForFullRotation / 4;
    }
}
