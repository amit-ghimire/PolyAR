using System;
using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;

/// <summary>
/// Loads the Poly Assets thumbnail images to a layout group
/// </summary>
public class ThumbnailLoader : MonoBehaviour
{
    #region Private Variables
    // The panel to load thumbnails to
    [SerializeField]
    [Tooltip("Panel to display the thumbnails")]
    private RectTransform assetsPanel;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        Application.runInBackground = true;
    }
    #endregion

    #region Public Methods
    public void LoadThumbnails() 
    {
        //Make a request to Poly to list asset
        //TODO : add request configuration
        PolyListAssetsRequest request = new PolyListAssetsRequest();
        request.category = PolyCategory.UNSPECIFIED;
        PolyApi.ListAssets(request, ListAssetCallback);
    }
    #endregion

    #region Poly Toolkit Callbacks
    /// <summary>
    /// Called when Asset is listed
    /// </summary>
    /// <param name="result">result of the request to list poly toolkit asset</param>
    private void ListAssetCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            Toaster.showToast("Failed to load asset : " + result.Status,2);
            return;
        }
        for (int i = 0; i < result.Value.assets.Count; i++) 
        {
            // Fetch thumbnails for assets listed by the request
            PolyApi.FetchThumbnail(result.Value.assets[i], FetchThumbnailCallback);
        }
    }

    /// <summary>
    /// Called when thumbnail is fetched
    /// </summary>
    /// <param name="asset">the asset of which thumbnail fetch was requested</param>
    /// <param name="status">status of the fetch request</param>
    private void FetchThumbnailCallback(PolyAsset asset, PolyStatus status) 
    {
        if (!status.ok) 
        {
            Toaster.showToast("Error fetching thumbnail : " + status.ToString(),2);
            return;
        }
        
        Texture2D texture = asset.thumbnailTexture;                     // store thumbnail texture
        GameObject thumbnailObject = new GameObject("thumbnail");       // instantiate gameobject to store thumbnail
        thumbnailObject.transform.SetParent(assetsPanel);               // make the gameobject children to assetpanel
        thumbnailObject.AddComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero); // add texture as sprite
        thumbnailObject.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width,texture.height); // resize the object
        thumbnailObject.AddComponent<Button>().onClick.AddListener(() => OnAssetThumbnailClicked(asset)); // add button to the object and add listener
    }
    #endregion

    #region Thumbnail Click Listener
    /// <summary>
    /// Handles click of the thumbnail button
    /// </summary>
    /// <param name="asset">The asset whose thumbnail is shown in the button</param>
    private void OnAssetThumbnailClicked(PolyAsset asset) 
    {
        AssetLoader.ImportAsset(asset);   
    }
    #endregion
}
