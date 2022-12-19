using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonType
{
    ChangeScene,
    UINavigation,
    LeaveGame
}

[RequireComponent(typeof(Button))]
public class CanvasSwitcher : MonoBehaviour
{
    public ButtonType buttonType;
    public CanvasType desiredCanvasType;
    public string desiredSceneName;
    
    private CanvasManager _canvasManager;
    private Button _menuButton;


    private void OnEnable()
    {
        _menuButton.onClick.AddListener(OnButtonClicked);
    }
    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(OnButtonClicked);
    }


    // Start is called before the first frame update
    void Awake()
    {
        _menuButton = GetComponent<Button>();
        _canvasManager = CanvasManager.GetInstance();
    }
    
    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case ButtonType.UINavigation:
                _canvasManager.SwitchCanvas(desiredCanvasType);
                break;
            case ButtonType.ChangeScene:
                SceneManager.LoadScene(SceneManager.GetSceneByName(desiredSceneName).buildIndex);
                Debug.Log("Change Scene to " + desiredSceneName);
                break;
            case ButtonType.LeaveGame:
                Debug.Log("Leaving Game");
                Application.Quit();
                break;
        }
    }
}
