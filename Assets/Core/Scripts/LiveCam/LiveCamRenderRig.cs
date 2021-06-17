using UnityEngine;
using UnityEngine.UI;

namespace kodai100.LiveCamCore
{
    public class LiveCamRenderRig : MonoBehaviour
    {
        [SerializeField] private RawImage image;

        public void SetTexture(RenderTexture renderTexture)
        {
            image.texture = renderTexture;
        }
    }
}