using kodai100.LiveCamCore;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    [SerializeField] private LiveCamController controller;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var camId = Mathf.FloorToInt(Random.Range(0f, controller.LiveCamNum));
            controller.TriggerNextLiveCamWithIndex(camId, LiveCamTriggerMode.Blending);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var camId = Mathf.FloorToInt(Random.Range(0f, controller.LiveCamNum));
            controller.TriggerNextLiveCamWithIndex(camId, LiveCamTriggerMode.CutIn);
        }
    }
}