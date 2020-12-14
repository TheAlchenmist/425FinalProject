using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousExplosion : MonoBehaviour
{
    public DoorOpener causeTrigger;
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (causeTrigger.doorOpened)
        {
            particles.Stop();
        }
    }
}
