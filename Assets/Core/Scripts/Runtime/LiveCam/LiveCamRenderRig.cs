using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

namespace kodai100.LiveCamCore
{
    [Serializable]
    public class DisplayInfo
    {
        public int display1;
        public int display2;
    }

    public class LiveCamRenderRig : MonoBehaviour
    {
        [SerializeField] private RawImage editorImage;
        [SerializeField] private RawImage outImage;

        private string JsonFilePath = Application.streamingAssetsPath + "/Display.json";

        private bool multiDisplay;

        private DisplayInfo info;

        private void Start()
        {
            info = Read();
            Debug.Log($"Display {info.display1}, {info.display2}");
        }

        public DisplayInfo Read()
        {
            var jsonDeserializedData = new DisplayInfo();

            try
            {
                using var fs = new FileStream(JsonFilePath, FileMode.Open);
                using var sr = new StreamReader(fs);

                var result = sr.ReadToEnd();

                jsonDeserializedData = JsonUtility.FromJson<DisplayInfo>(result);
            }
            catch (Exception e)
            {
                Debug.LogError("Display Read Error");
            }

            return jsonDeserializedData;
        }

        public void SetTexture(RenderTexture renderTexture)
        {
            editorImage.texture = renderTexture;
            outImage.texture = renderTexture;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!multiDisplay)
                {
                    if (Display.displays.Length > 1)
                    {
                        Display.displays[info.display1].Activate();
                        Display.displays[info.display2].SetParams(1280, 720, 0, 0);
                        Screen.SetResolution(1280, 720, false);
                    }
                }
            }
        }
    }
}