using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

namespace kodai100.LiveCamCore
{
#if MULTI_DISPLAY
    [Serializable]
    public class DisplayInfo
    {
        public int display1;
        public int display2;
    }
#endif

    public class LiveCamRenderRig : MonoBehaviour
    {
        [SerializeField] private RawImage editorImage;
        [SerializeField] private RawImage outImage;


        private void Start()
        {
#if MULTI_DISPLAY
            info = Read();
            Debug.Log($"Display {info.display1}, {info.display2}");
#endif
        }

        public void SetTexture(RenderTexture renderTexture)
        {
            editorImage.texture = renderTexture;
            outImage.texture = renderTexture;
        }

        public void Update()
        {
#if MULTI_DISPLAY
           MultiDisplayUpdate();
#endif
        }


#if MULTI_DISPLAY
        private string JsonFilePath = Application.streamingAssetsPath + "/Display.json";

        private DisplayInfo info;
        
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
        
        private void MultiDisplayUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (Display.displays.Length > 1)
                {
                    Display.displays[info.display1].Activate();
                    Display.displays[info.display2].SetParams(1280, 720, 0, 0);
                    Screen.SetResolution(1280, 720, false);
                }
            }
        }
#endif
    }
}