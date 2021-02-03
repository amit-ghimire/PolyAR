using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARLesson : MonoBehaviour
{
    public ARPlaneManager arPlaneManager;
    // Start is called before the first frame update
    void Start()
    {
        arPlaneManager.planesChanged += OnPlaneChanged;
    }

    private void OnPlaneChanged(ARPlanesChangedEventArgs obj)
    {
        Debug.Log(obj.added.Count.ToString());
    }
}
