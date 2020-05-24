using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public int cameraSpeed = 10;

    [Header("Objects")]
    public CameraHolder holder;

    // Start is called before the first frame update
    void Start()
    {
        holder = GetComponentInChildren<CameraHolder>();
    }

    public void MoveCamera(Vector3 direction)
    {
        Vector3 dir = direction * cameraSpeed * Time.deltaTime;
        transform.Translate(dir);
        ClampToScenarioBounds();
    }

    public void PlaceCamera(Vector3 position)
    {
        Vector3 holderFix = holder.transform.localPosition;
        //TODO: have an function to get half the tile size instead of hardcoding it;
        holderFix.x -= 0.5F;
        holderFix.y = 0;
        holderFix.z -= 0.5F;
        position -= holderFix;
        transform.position = position;
        ClampToScenarioBounds();
    }

    private void ClampToScenarioBounds()
    {
        ScenarioManager sm = ScenarioManager.Instance;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 0, sm.scenario.scenarioSize.x - sm.MIN_SIZE.x);
        pos.z = Mathf.Clamp(pos.z, 0, (sm.scenario.scenarioSize.y - sm.MIN_SIZE.y) + 1);
        transform.position = pos;
    }
}
