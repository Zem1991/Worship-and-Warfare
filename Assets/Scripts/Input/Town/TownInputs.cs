using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class TownInputs : AbstractSingleton<TownInputs>, IInputScheme, IShowableHideable
{
    [Header("Required Objects")]
    public InputManager im;
    public TownInputRecorder recorder;
    public CameraController cameraController;

    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        im = InputManager.Instance;
        recorder = GetComponent<TownInputRecorder>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public CameraController CameraController()
    {
        return cameraController;
    }

    public void UpdateInputs()
    {
        //CameraControls();
        //CursorHighlight();

        //SelectionHighlight();
        //SelectionCommand();
    }
}
