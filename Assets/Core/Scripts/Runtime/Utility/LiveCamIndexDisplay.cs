using UnityEngine;
using UnityEngine.UI;

namespace kodai100.LiveCamCore
{
    public class LiveCamIndexDisplay : MonoBehaviour
    {
        [SerializeField] private Text text;

        private LiveCamController controller;

        private void Start()
        {
            controller = FindObjectOfType<LiveCamController>();
        }

        private void Update()
        {
            if (controller == null) return;

            text.text = $"CAM {controller.CurrentLiveCamIndex:D2} / {controller.LiveCamNum:D2}";
        }
    }
}