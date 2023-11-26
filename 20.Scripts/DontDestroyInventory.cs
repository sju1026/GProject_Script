using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyInventory : MonoBehaviour
{
    private static DontDestroyInventory i_Instance = null;

    private void Awake()
    {
        if (i_Instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        i_Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
}
