using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public static ParticleController Instance;

    [SerializeField]
    private ParticleSystem particleHit = null;

    [SerializeField]
    private ParticleSystem particleMiss = null;

    void Start()
    {
        Instance = this;
    }

    // Emit assigned particle
    public void EmitParticle(int count, Indicator indicator)
    {
        switch (indicator)
        {
            case Indicator.Hit:
                particleHit.Emit(count);
                break;
            case Indicator.Miss:
                particleMiss.Emit(count);
                break;
        }
    }

    public enum Indicator
    {
        Hit,
        Miss,
    }
}
