using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    StartGame,
    UINavigation,
    CreateRoom,
    JoinRoom,
    LeaveGame
}

[RequireComponent(typeof(Button))]
public class CanvasSwitcher : MonoBehaviour
{
    public ButtonType buttonType;
    
    public CanvasType desiredCanvasType;

    private CanvasManager _canvasManager;
    private Button _menuButton;
    
    // Start is called before the first frame update
    void Start()
    {
        _menuButton = GetComponent<Button>();
        _menuButton.onClick.AddListener(OnButtonClicked);
        _canvasManager = CanvasManager.GetInstance();
    }

    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case ButtonType.UINavigation:
                _canvasManager.SwitchCanvas(desiredCanvasType);
                break;
            case ButtonType.StartGame:
                //Call Scene manager to load the game scene
                Debug.Log("LunchGame");
                break;
            case ButtonType.CreateRoom:
                //Call network manager logic to create a room
                break;
            case ButtonType.JoinRoom:
                //Call network manager logic to join a room
                break;
            case ButtonType.LeaveGame:
                Debug.Log("Leaving Game");
                Application.Quit();
                break;
        }
    }
}
