using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputScheme
{
    CameraController CameraController();
    void UpdateInputs();
}
