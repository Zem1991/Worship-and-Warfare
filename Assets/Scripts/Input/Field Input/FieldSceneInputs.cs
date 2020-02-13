using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSceneInputs : AbstractSingleton<FieldSceneInputs>, IShowableHideable, ISceneInputs
{
    public FieldInputListener listener;
    public FieldInputInterpreter interpreter;
    public FieldInputExecutor executor;

    public override void Awake()
    {
        listener = GetComponent<FieldInputListener>();
        interpreter = GetComponent<FieldInputInterpreter>();
        executor = GetComponent<FieldInputExecutor>();

        base.Awake();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ClearInputs()
    {
        interpreter.ClearInputs();
    }

    public void ExecuteInputs()
    {
        executor.ExecuteInputs();
    }

    public CameraController GetCameraController()
    {
        return executor.cameraController;
    }
}
