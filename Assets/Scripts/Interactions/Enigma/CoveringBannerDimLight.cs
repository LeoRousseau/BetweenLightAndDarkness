using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoveringBannerDimLight : MonoBehaviour
{
    public Light associatedLight;
    public float newIntensity;
    // Start is called before the first frame update
    void Start()
    {
        associatedLight.intensity = newIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
