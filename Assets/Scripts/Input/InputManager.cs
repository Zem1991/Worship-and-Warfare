using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Singleton;
    private InputListener listener;

    public Vector3 cursorAxes;
    public Vector3 cameraAxes;
    public bool selectionDown;
    public bool commandDown;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("Only one instance of InputManager may exist! Deleting this extra one.");
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<InputListener>();
    }

    // Update is called once per frame
    void Update()
    {
        cursorAxes = listener.CursorAxes();
        cameraAxes = listener.CameraAxes();
        selectionDown = listener.SelectionDown();
        commandDown = listener.CommandDown();
    }
}
