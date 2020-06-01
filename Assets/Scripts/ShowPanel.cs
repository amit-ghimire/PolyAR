using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    private Button button;

    [SerializeField]
    private GameObject scrollPanel;

    private bool isActive;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        isActive = false;
        button.onClick.AddListener(() => OnButtonPressed());
    }

    void OnButtonPressed() 
    {
        scrollPanel.SetActive(!isActive);
        scrollPanel.GetComponent<ThumbnailLoader>().LoadThumbnails();
        isActive = !isActive;
    }
}
