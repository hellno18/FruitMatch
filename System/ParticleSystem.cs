using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{

    [SerializeField] private ParticleSystem[] allParticles;
    [SerializeField] private float lifetime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        allParticles = GetComponentsInChildren<ParticleSystem>();
        Destroy(gameObject, lifetime);
        
    }

    public void Play()
    {
         foreach (ParticleSystem particle in allParticles)
        {
            particle.Play();
        }
    }
}
