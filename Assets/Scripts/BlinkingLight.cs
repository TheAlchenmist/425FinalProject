using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    private new Light light;
    float InitialIntensity = 0;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        InitialIntensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = Mathf.PingPong(Time.time, InitialIntensity);
    }
}
