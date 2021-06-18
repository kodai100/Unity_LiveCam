using kodai100.LiveCamCore;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    [SerializeField] private LiveCamController controller;

    private int prevIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            while (true)
            {
                var camId = Mathf.FloorToInt(Random.Range(0f, controller.LiveCamNum));

                if (camId == prevIndex) continue;

                prevIndex = camId;
                controller.TriggerNextLiveCamWithIndex(camId, LiveCamTriggerMode.Blending, Random.Range(0f, 1f));
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            while (true)
            {
                var camId = Mathf.FloorToInt(Random.Range(0f, controller.LiveCamNum));

                if (camId == prevIndex) continue;

                prevIndex = camId;
                controller.TriggerNextLiveCamWithIndex(camId, LiveCamTriggerMode.CutIn, 0);
                break;
            }
        }
    }
}