using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputScheme
{
    CameraController CameraController();
    bool IsGamePaused();
    bool IsCursorValid();
    void UpdateInputs();
}
