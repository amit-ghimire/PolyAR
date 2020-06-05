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
    #region Singleton
    private static AssetLoader _instance;
    
    public static AssetLoader Instance 
    {
        get 
        {
            return _instance;
        }
    }
    #endregion

    #region Private Variables
    [SerializeField]
    [Tooltip("The Scroll Panel")]
    private GameObject scrollPanel;
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
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Import asset from clicked thumbnail button
    /// </summary>
    /// <param name="asset">The asset to be imported</param>
    public static void ImportAsset(PolyAsset asset)
    {
        PolyImportOptions options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 0.2f;
        options.recenter = true;

        PolyApi.Import(asset, options, ImportAssetCallback);
        Instance.scrollPanel.SetActive(false);
    }
    #endregion

    #region Poly Toolkit Callbacks
    /// <summary>
    /// Called after asset is imported to the scene
    /// </summary>
    /// <param name="asset">The asset imported to the scene</param>
    /// <param name="result">result of the import request => gameobject / error</param>
    private static void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            Toaster.showToast("Failed to import asset. Reason: " + result.Status, 2);
            return;
        }
        GameObject importedAsset = result.Value.gameObject;
        AssetPlacer.setupAsset(importedAsset);    
    }
    #endregion
}
