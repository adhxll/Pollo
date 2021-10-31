using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public static ParticleController Instance;

    [SerializeField]
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        particles = GetComponent<ParticleSystem>();
    }

    public void EmitParticle(int count)
    {
        if (particles != null)
            particles.Emit(count);
    }
}
