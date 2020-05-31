using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The folder where images will be loaded from")]
    private string imagePath;

    [SerializeField]
    [Tooltip("The panel where new images will be added as children")]
    private RectTransform assetsPanel;

    private List<Texture2D> textures;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(LoadImages());
    }

    public IEnumerator LoadImages() 
    {
        textures = new List<Texture2D>();

        DirectoryInfo di = new DirectoryInfo(imagePath);
        var files = di.GetFiles("*.jpg");

        foreach (var file in files)
        {
            Toaster.Instance.showToast(file.FullName,2);
            yield return LoadTextureAsync(file.FullName, AddLoadedTextureToCollection);
        }

        CreateImages();
    }

    private void AddLoadedTextureToCollection(Texture2D texture) 
    {
        textures.Add(texture);
    }

    private void CreateImages() 
    {
        foreach(var texture in textures) 
        {
            GameObject imageObject = new GameObject("Image");
            imageObject.transform.SetParent(assetsPanel);
            imageObject.AddComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 1920);
        }
    }

    public IEnumerator LoadTextureAsync(string originalFileName, Action<Texture2D> result) 
    {
        string fileToLoad = GetCleanFileName(originalFileName);
        Toaster.Instance.showToast("Loading Image from path:" + fileToLoad, 2);

        WWW www = new WWW(fileToLoad);
        yield return www;

        Texture2D loadedTexture = new Texture2D(1, 1);
        www.LoadImageIntoTexture(loadedTexture);
        result(loadedTexture);
    }


    private static string GetCleanFileName(string originalFileName)
    {
        string fileToLoad = originalFileName.Replace('\\', '/');

        if (fileToLoad.StartsWith("http") == false)
        {
            fileToLoad = string.Format("file://{0}", fileToLoad);
        }

        return fileToLoad;
    }
}
