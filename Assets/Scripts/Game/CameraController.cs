using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public int cameraSpeed = 10;

    [Header("Objects")]
    public new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCamera(Vector3 direction)
    {
        ScenarioManager sm = ScenarioManager.Singleton;

        Vector3 dir = direction * cameraSpeed * Time.deltaTime;
        transform.Translate(dir);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 0, sm.scenario.scenarioSize.x - sm.MIN_SIZE.x);
        pos.z = Mathf.Clamp(pos.z, 0, sm.scenario.scenarioSize.y - sm.MIN_SIZE.y);
        transform.position = pos;
    }
}
