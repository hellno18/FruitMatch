using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject clearFXPrefap;
    [SerializeField] private GameObject breakFXPrefap;

    public void ClearDotFxAt(int x, int y, int z = 0)
    {
        if (clearFXPrefap != null)
        {
            GameObject clearFx = Instantiate(clearFXPrefap, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            ParticleSystem particlesystem = clearFx.GetComponent<ParticleSystem>();
            if (particlesystem != null)
            {
                particlesystem.Play();
            }
        }

    }
    public void BreakDotFxAT(int breakAbleValue,int x, int y, int z = 0)
    {
        GameObject breakFx = null;
        ParticleSystem particlesystem = null;
        if (breakFXPrefap != null)
        {
            breakFx = Instantiate(breakFXPrefap, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        }
        if (breakFx == null)
        {
            particlesystem = breakFx.GetComponent<ParticleSystem>();
            if (particlesystem != null)
            {
                particlesystem.Play(); 
            }
        }
        
    }
}
