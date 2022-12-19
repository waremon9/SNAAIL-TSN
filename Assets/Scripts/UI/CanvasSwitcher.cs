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
    CreateRoom,
    JoinRoom,
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
            case ButtonType.ChangeScene:
                SceneManager.LoadScene(SceneManager.GetSceneByName(desiredSceneName).buildIndex);
                Debug.Log("Change Scene to " + desiredSceneName);
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
