using PolyToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Utility class to import asset
/// and pass on imported asset's gameobject
/// to assetPlacer
/// </summary>
public class AssetLoader : MonoBehaviour
{
    public static AssetLoader _instance;
    
    [SerializeField]
    [Tooltip("The Scroll Panel")]
    private GameObject scrollPanel;

    public static AssetLoader Instance 
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
    }

    public static void ImportAsset(PolyAsset asset)
    {
        PolyImportOptions options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 1.0f;
        options.recenter = true;

        PolyApi.Import(asset, options, ImportAssetCallback);
        Instance.scrollPanel.SetActive(false);
    }

    private static void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            Toaster.Instance.showToast("Failed to import asset. Reason: " + result.Status, 2);
            return;
        }
        GameObject importedAsset = result.Value.gameObject;
        importedAsset.transform.SetParent(GameObject.Find("Trackables").transform);
        AssetPlacer.placeOnPlane(importedAsset);
    }

}
