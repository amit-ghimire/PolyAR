using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Toggle Asset Browser Panel
/// </summary>
public class ShowPanel : MonoBehaviour
{
    #region Private Variables
    //The button to toggle the panel
    private Button button;
    
    //The Panel
    [SerializeField]
    private GameObject scrollPanel;

    //Is showing ?
    private bool isActive;
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        button = GetComponent<Button>();
        isActive = false;
        button.onClick.AddListener(() => OnButtonPressed());
    }
    #endregion

    #region Button Click Listener
    void OnButtonPressed() 
    {
        scrollPanel.SetActive(!isActive);
        scrollPanel.GetComponent<ThumbnailLoader>().LoadThumbnails();
        isActive = !isActive;
    }
    #endregion
}
