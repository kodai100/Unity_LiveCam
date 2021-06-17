using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace kodai100.LiveCamCore
{
    public enum LiveCamTriggerMode
    {
        CutIn,
        Blending
    }

    public class Slot
    {
        public bool IsSlotA = false;
        public RenderTexture RenderTexture;
        public float MyGloal;

        public Slot(bool isSlotA)
        {
            RenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBHalf)
            {
                name = isSlotA ? "SlotA" : "SlotB"
            };
            RenderTexture.Create();

            MyGloal = isSlotA ? 0f : 1f;
        }

        public void SetCamera(LiveCam cam)
        {
            cam.SetTexture(RenderTexture);
        }
    }

    public class LiveCamController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float blending = 0f;

        [SerializeField] private LiveCamRenderRig liveCamRenderRigPrefab;

        [SerializeField] private Material blendingMaterial;

        [SerializeField] private float blendingDuration = 1f;

        private Slot slotA;
        private Slot slotB;

        private Slot currentSlot;
        private Slot vacantSlot;

        private RenderTexture resultRenderTexture;

        private List<LiveCam> liveCamList;

        private LiveCam currentActiveCamera;

        private bool isOperating;

        public int LiveCamNum => liveCamList.Count;

        private void Start()
        {
            slotA = new Slot(true);
            slotB = new Slot(false);

            resultRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBHalf);
            resultRenderTexture.Create();

            liveCamList = Resources.FindObjectsOfTypeAll<LiveCam>().ToList();

            if (liveCamList.Count == 0)
            {
                Debug.Log("There is no Live Cam on this scene. Aborted");
                Application.Quit();
            }

            liveCamList.ForEach(cam =>
            {
                cam.gameObject.SetActive(true);
                cam.Deactivate();
            });

            currentSlot = slotA;
            vacantSlot = slotB;

            TriggerNextLiveCam(liveCamList[0], LiveCamTriggerMode.CutIn);

            var rig = Instantiate(liveCamRenderRigPrefab);
            rig.SetTexture(resultRenderTexture);
        }

        public async void TriggerNextLiveCam(LiveCam cam, LiveCamTriggerMode mode)
        {
            if (isOperating)
            {
                // Debug.Log("Under Operation. Aborted");
                return;
            }

            if (cam == currentActiveCamera)
            {
                // Debug.Log("Target is current camera. Aborted.");
                return;
            }

            if (mode == LiveCamTriggerMode.Blending)
            {
                cam.Activate();

                vacantSlot.SetCamera(cam);

                await BlendingCoroutine(currentSlot, vacantSlot);

                currentActiveCamera?.Deactivate();
                currentActiveCamera = cam;

                var tmp = currentSlot;
                currentSlot = vacantSlot;
                vacantSlot = tmp;
            }
            else
            {
                cam.Activate();
                currentSlot.SetCamera(cam);
                currentActiveCamera?.Deactivate();
                currentActiveCamera = cam;
                blending = currentSlot.MyGloal;
            }
        }

        public void TriggerNextLiveCamWithIndex(int index, LiveCamTriggerMode mode)
        {
            if (index < liveCamList.Count)
            {
                TriggerNextLiveCam(liveCamList[index], mode);
            }
        }

        private IEnumerator BlendingCoroutine(Slot from, Slot dst)
        {
            isOperating = true;

            var t = 0f;

            while (true)
            {
                if (t > blendingDuration) break;

                t += Time.deltaTime;

                blending = Mathf.Lerp(from.MyGloal, dst.MyGloal, t / blendingDuration);

                yield return null;
            }

            blending = dst.MyGloal;

            isOperating = false;
        }

        private void Update()
        {
            blendingMaterial.SetFloat("_Blending", blending);
            blendingMaterial.SetTexture("_SrcOne", slotA.RenderTexture);
            blendingMaterial.SetTexture("_SrcTwo", slotB.RenderTexture);
            Graphics.Blit(null, resultRenderTexture, blendingMaterial);
        }
    }
}