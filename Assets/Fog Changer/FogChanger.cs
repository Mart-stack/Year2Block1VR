using System;
using Unity.VisualScripting;
using UnityEngine;

public class FogChanger : MonoBehaviour
{
    public bool outside;
    readonly float fogOutside = .075f;
    readonly float fogInside = .015f;

    private void OnTriggerStay(Collider other)
    {
        if (outside)    //outside
        {
            if (RenderSettings.fogDensity > fogOutside) return;
            RenderSettings.fogDensity += .001f;
            Debug.Log("Increasing fog density");
        }
        else            //inside
        {
            if (RenderSettings.fogDensity < fogInside) return;
            RenderSettings.fogDensity -= .001f;
            Debug.Log("Decreasing fog density");
        }
    }
}
