using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcticleSortingLayerSet : MonoBehaviour
{
    public string sortingLayerName;

    void Start()
    {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
    }
}
