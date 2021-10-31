using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    public ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles.Stop();
    }

    public void TriggerParticle()
    {
        if (particles != null)
            particles.Emit(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
