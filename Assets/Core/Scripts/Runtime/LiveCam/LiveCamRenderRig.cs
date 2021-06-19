using UnityEngine;
using UnityEngine.UI;

namespace kodai100.LiveCamCore
{
    public class LiveCamRenderRig : MonoBehaviour
    {
        [SerializeField] private RawImage editorImage;
        [SerializeField] private RawImage outImage;

        private bool multiDisplay;

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
                        Display.displays[1].Activate();
                        Display.displays[0].SetParams(1280, 720, 0, 0);
                        Screen.SetResolution(1280, 720, false);
                    }
                }
            }
        }
    }
}