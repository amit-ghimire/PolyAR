using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Utility class that handles placement of 
/// imported asset's gameobject in the AR world
/// </summary>
public class AssetPlacer : MonoBehaviour
{
    public static AssetPlacer _instance;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    [SerializeField]
    [Tooltip("The AR Session Origin to get reference of ARRaycastManager")]
    private GameObject arSessionOrigin;

    [SerializeField]
    private ARRaycastManager m_RaycastManager;

    [SerializeField]
    private GameObject loadedAsset;
    public static AssetPlacer Instance 
    {
        get 
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        m_RaycastManager = arSessionOrigin.GetComponent<ARRaycastManager>();
    }

    static bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    public static void placeOnPlane(GameObject go) 
    {
        Instance.loadedAsset = go;
            
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            Debug.Log("out of here");
            return;
        }

        if (Instance.m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            Debug.Log(go.name);
            var hitPose = s_Hits[0].pose;
            go.transform.position = hitPose.position;
        }
        else 
        {
            Debug.Log("No raycast");
        }
    }

    private void Update()
    {
        if (Instance.loadedAsset != null) 
        {
            placeOnPlane(Instance.loadedAsset);
        }
    }

}
