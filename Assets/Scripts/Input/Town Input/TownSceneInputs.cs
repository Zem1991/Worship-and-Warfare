using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSceneInputs : AbstractSingleton<TownSceneInputs>, IShowableHideable, ISceneInputs
{
    public TownInputListener listener;
    public TownInputInterpreter interpreter;
    public TownInputExecutor executor;

    public override void Awake()
    {
        listener = GetComponent<TownInputListener>();
        interpreter = GetComponent<TownInputInterpreter>();
        executor = GetComponent<TownInputExecutor>();

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
