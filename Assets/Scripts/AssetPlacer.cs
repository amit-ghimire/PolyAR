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
    #region Singleton
    private static AssetPlacer _instance;
    public static AssetPlacer Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    #region Private Variables

    [SerializeField]
    [Tooltip("The AR Session Origin to get reference of ARRaycastManager and ARCamera")]
    private GameObject arSessionOrigin;

    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>(); // Store Raycast hits from AR Raycast

    private ARRaycastManager m_RaycastManager;                          // Handles AR Raycast 
    
    private Camera arCamera;                                            // The AR Camera

    private Vector2 touchPosition;                                      // Touch postion in screen 

    private ARAsset selectedAsset;                                      // handle placement of this asset
    #endregion

    #region Public Variables
    public List<ARAsset> loadedAssets;                                  // store all the assets loaded in AR scene
    #endregion

    #region Unity Callbacks
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
        arCamera = arSessionOrigin.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                selectAsset(touchPosition);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                selectedAsset.selected = false;
            }
        }
        placeOnPlane(touchPosition);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Initial setup of ARAsset after imported in the scene
    /// </summary>
    /// <param name="asset">The asset imported</param>
    public static void setupAsset(GameObject asset)
    {
        asset.transform.SetParent(GameObject.Find("Trackables").transform);     // make it a child of tracked plane in AR scene
        asset.AddComponent<ARAsset>();                                          // Add ARAsset component to handle selection
        var assetCollider = (BoxCollider)asset.AddComponent<BoxCollider>();     // Add Collider to handle Raycast 
        assetCollider.size = new Vector3(0.15f, 0.15f, 0.15f);                  // TODO : Fine tune Shape/size of collider
        Instance.loadedAssets.Add(asset.GetComponent<ARAsset>());               // Add to loadedAssets list for handling selection/deselection
        Instance.selectedAsset = asset.GetComponent<ARAsset>();                 // assign this asset to selected
        Instance.selectedAsset.selected = true;                                 // Make it selected initially after import to enable placement
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Handle Selection of AR assets based on Unity raycast
    /// </summary>
    /// <param name="touchPosition">Current touch position in screen</param>
    private void selectAsset(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);                     // Unity Raycast with AR camera
        RaycastHit hitObject;
        if (Physics.Raycast(ray, out hitObject))
        {
            selectedAsset = hitObject.transform.GetComponentInChildren<ARAsset>();
            if (selectedAsset != null)                                          // if Raycast hit an ARAsset
            {
                foreach (ARAsset asset in loadedAssets)
                {
                    asset.selected = asset == selectedAsset;                    // assign ARAsset hit with raycast to selected
                }
            }
        }
    }
    /// <summary>
    /// Handle Placement of AR assets based on AR Raycast
    /// </summary>
    /// <param name="touchPosition">Current touch position in screen</param>
    private void placeOnPlane(Vector2 touchPosition) 
    {
        if (Instance.m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon)) //AR Raycast from touch position => check if plane is hit 
        {
            var hitPose = s_Hits[0].pose;
            if(selectedAsset == null)
            {
                //TODO: handle null case selection
            }
            else
            {
                if(selectedAsset.selected)
                    selectedAsset.transform.localPosition = hitPose.position;                           // Place the asset to hit pose of ARRaycast on plane
            }
        }
    }
    #endregion
}
