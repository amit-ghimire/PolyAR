using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Checks various state of AR Session
/// Attemps install of AR software required in device or
/// Reports unavailiability of AR in device
/// </summary>
public class DeviceSupport : MonoBehaviour
{
    #region Unity Callbacks

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        ARSession.stateChanged += onSessionStateChange;
    }
    #endregion

    #region ARSession state change callback
    private void onSessionStateChange(ARSessionStateChangedEventArgs obj)
    {
        switch (obj.state)
        {
            case ARSessionState.CheckingAvailability:
                Toaster.showToast("Checking Availability", 2);
                break;
            case ARSessionState.NeedsInstall:
                Toaster.showToast("Needs Install",2);
                break;
            case ARSessionState.Installing:
                Toaster.showToast("Installing",2);
                break;
            case ARSessionState.Ready:
                Toaster.showToast("Ready",2);
                break;
            case ARSessionState.SessionInitializing:
                Toaster.showToast("Initialiazing session", 2);
                break;
            case ARSessionState.SessionTracking:
                Toaster.showToast("Tracking", 2);
                break;
            case ARSessionState.Unsupported:
                Toaster.showToast("AR not supported",2);
                break;
        }
    }
    #endregion
}
