using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using System;

public class DeviceSupport : MonoBehaviour
{
    [SerializeField]
    ARSession m_session;

    private void OnEnable()
    {
        ARSession.stateChanged += onSessionStateChange;
    }

    private void onSessionStateChange(ARSessionStateChangedEventArgs obj)
    {
        switch (obj.state)
        {
            case ARSessionState.CheckingAvailability:
                Toaster.Instance.showToast("Checking Availability", 2);
                break;
            case ARSessionState.NeedsInstall:
                Toaster.Instance.showToast("Needs Install",2);
                break;
            case ARSessionState.Installing:
                Toaster.Instance.showToast("Installing",2);
                break;
            case ARSessionState.Ready:
                Toaster.Instance.showToast("Ready",2);
                break;
            case ARSessionState.SessionInitializing:
                Toaster.Instance.showToast("Initialiazing session", 2);
                break;
            case ARSessionState.SessionTracking:
                Toaster.Instance.showToast("Tracking", 2);
                break;
            case ARSessionState.Unsupported:
                Toaster.Instance.showToast("AR not supported",2);
                break;
        }
    }
}
