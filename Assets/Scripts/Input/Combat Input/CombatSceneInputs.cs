using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneInputs : AbstractSingleton<CombatSceneInputs>, IShowableHideable, ISceneInputs
{
    public CombatInputListener listener;
    public CombatInputInterpreter interpreter;
    public CombatInputExecutor executor;

    public override void Awake()
    {
        listener = GetComponent<CombatInputListener>();
        interpreter = GetComponent<CombatInputInterpreter>();
        executor = GetComponent<CombatInputExecutor>();

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
        return executor.GetCameraController();
    }
}
