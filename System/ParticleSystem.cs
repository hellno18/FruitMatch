using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{

    public ParticleSystem[] allParticles;
    public float lifetime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        allParticles = GetComponentsInChildren<ParticleSystem>();
        Destroy(gameObject, lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Play()
    {
         foreach (ParticleSystem particle in allParticles)
        {
            particle.Play();
        }
    }
}
