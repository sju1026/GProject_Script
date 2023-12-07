using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool isLight;
    public Light lighting;

    private void Awake()
    {
        lighting = GetComponent<Light>();
    }

    private void Update()
    {
        if (isLight == true)
        {
            lighting.enabled = true;
        }
        else if (isLight == false)
        {
           lighting.enabled = false;
        }
    }
}
