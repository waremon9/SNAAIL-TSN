using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    MainMenu,
    InGameMenu,
    Settings,
    CreateRoom,
    JoinRoom
}

public class CanvasManager : Singleton<CanvasManager>
{
    private List<CanvasController> _canvasControllerList;
    private CanvasController _lastActiveCanvas;
    protected override void Awake()
    {
        _canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        _canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.MainMenu);
    }

    public void SwitchCanvas(CanvasType type)
    {
        if (_lastActiveCanvas != null)
        {
            _lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = _canvasControllerList.Find(x => x.canvasType == type);

        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            _lastActiveCanvas = desiredCanvas;
        }
        else
        {
            Debug.LogWarning("The desired canvas was not found");
        }
    }

}
