using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInputExecutor<T, U> : MonoBehaviour where T : AbstractInputInterpreter<U> where U : AbstractInputListener
{
    protected InputManager im;
    protected T interpreter;

    protected CameraController cameraController;

    public virtual void Awake()
    {
        im = InputManager.Instance;
        interpreter = GetComponent<T>();

        cameraController = GetComponentInChildren<CameraController>();
    }

    public CameraController GetCameraController()
    {
        return cameraController;
    }

    public bool IsGamePaused()
    {
        return GameManager.Instance.isPaused;
    }

    public bool IsCursorValid()
    {
        return !UIManager.Instance.focusedPanel;
    }

    protected abstract void ManageWindows();
    protected abstract bool HasCurrentWindow();
    public abstract void ExecuteInputs();
}
