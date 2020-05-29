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
                break;
            case ARSessionState.NeedsInstall:
                ShowSessionStateMessage();
                break;
            case ARSessionState.Installing:
                ShowSessionStateMessage();
                break;
            case ARSessionState.Ready:
                ShowSessionStateMessage();
                break;
            case ARSessionState.SessionInitializing:
                ShowSessionStateMessage();
                break;
            case ARSessionState.SessionTracking:
                ShowSessionStateMessage();
                break;
            case ARSessionState.Unsupported:
                ShowSessionStateMessage();
                break;
        }
    }

    private void ShowSessionStateMessage() {
    
    }
}
