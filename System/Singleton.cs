using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//open constructed type
public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<T>();
                if(m_instance == null)
                {
                    GameObject singelton = new GameObject(typeof(T).Name);
                    m_instance = singelton.AddComponent<T>();
                }
            }
            return m_instance;
        }
    }

    public virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
