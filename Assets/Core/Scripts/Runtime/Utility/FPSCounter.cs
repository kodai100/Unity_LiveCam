using UnityEngine;
using UnityEngine.UI;

namespace kodai100.LiveCamCore
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private Text text;

        private float fps;
        private float updateInterval = 1.0f;
        private float accumulated = 0.0f;
        private int numFrames = 0;

        public void Update()
        {
            var deltaTime = Time.deltaTime;

            accumulated += Time.timeScale / deltaTime;
            ++numFrames;

            fps = accumulated / numFrames;
            accumulated = 0.0f;
            numFrames = 0;

            if (text)
            {
                text.text = $"FPS : {Mathf.FloorToInt(fps):D3}";
                text.color = fps < 30 ? Color.red : Color.cyan;
            }
        }
    }
}