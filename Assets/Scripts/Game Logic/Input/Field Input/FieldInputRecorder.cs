using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInputRecorder : MonoBehaviour
{
    private FieldInputListener listener;

    [Header("Inputs")]
    public Vector3 cursorAxes;
    public Vector3 cameraAxes;
    public bool escapeMenuDown;
    public bool selectionDown;
    public bool commandDown;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<FieldInputListener>();
    }

    // Update is called once per frame
    void Update()
    {
        cursorAxes = listener.CursorAxes();
        cameraAxes = listener.CameraAxes();
        escapeMenuDown = listener.EscapeMenuDown();
        selectionDown = listener.SelectionDown();
        commandDown = listener.CommandDown();
    }
}
