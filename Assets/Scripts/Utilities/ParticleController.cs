using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public static ParticleController Instance;

    [SerializeField]
    private ParticleSystem particles;

    void Start()
    {
        Instance = this;
        particles = GetComponent<ParticleSystem>();
    }

    // Emit assigned particle
    public void EmitParticle(int count)
    {
        if (particles != null)
            particles.Emit(count);
    }
}
