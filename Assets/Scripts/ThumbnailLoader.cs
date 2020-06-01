using System;
using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PolyToolkit;

public class ThumbnailLoader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Panel to display the thumbnails")]
    private RectTransform assetsPanel;


    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
    }

    public void LoadThumbnails() 
    {
        PolyListAssetsRequest request = new PolyListAssetsRequest();
        request.category = PolyCategory.UNSPECIFIED;
        PolyApi.ListAssets(request, ListAssetCallback);
    }
    private void ListAssetCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            Toaster.Instance.showToast("Failed to load asset : " + result.Status,2);
            return;
        }
        for (int i = 0; i < result.Value.assets.Count; i++) 
        {
            PolyApi.FetchThumbnail(result.Value.assets[i], FetchThumbnailCallback);
        }
    }

    private void FetchThumbnailCallback(PolyAsset asset, PolyStatus status) 
    {
        if (!status.ok) 
        {
            Toaster.Instance.showToast("Error fetching thumbnail : " + status.ToString(),2);
            return;
        }
        Texture2D texture = asset.thumbnailTexture;
        GameObject thumbnailObject = new GameObject("thumbnail");
        thumbnailObject.transform.SetParent(assetsPanel);
        thumbnailObject.AddComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        thumbnailObject.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width,texture.height);
        thumbnailObject.AddComponent<Button>().onClick.AddListener(() => onAssetThumbnailClicked(asset));
    }

    private void onAssetThumbnailClicked(PolyAsset asset) 
    {
        Toaster.Instance.showToast("Asset to import : " + asset.displayName.ToString(),2);
    }
}
