using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    public bool destroyOnAwake;
    public float awakeDestroyDelay;
    public bool findChild = false;
    public string namedChild;

    private void Awake()
    {
        if (destroyOnAwake)
        {
            if (findChild)
            {
                Destroy(transform.Find(namedChild).gameObject);
            }
            else
            {
                Destroy(gameObject, awakeDestroyDelay);
            }
        }
    }
    void DestroyChildGameObject()
    {
        if(transform.Find(namedChild).gameObject != null)
        {
            Destroy(transform.Find(namedChild).gameObject);
        }
    }

    void DisableChildGameObject()
    {
        if(transform.Find(namedChild).gameObject.activeSelf == true)
        {
            transform.Find(namedChild).gameObject.SetActive(false);
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
