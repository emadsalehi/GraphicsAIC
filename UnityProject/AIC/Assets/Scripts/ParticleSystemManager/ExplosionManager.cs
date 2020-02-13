using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public ParticleSystem explosionParticleSystem;

    public void PlayExplosion()
    {
        explosionParticleSystem.Play();
    }
}
