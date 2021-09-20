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
        public float Goal;

        public Slot(int width, int height, bool isSlotA)
        {
            RenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGBFloat)
            {
                name = isSlotA ? "SlotA" : "SlotB"
            };
            RenderTexture.Create();

            Goal = isSlotA ? 0f : 1f;
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

        [SerializeField] private Vector2Int screenResolution = new Vector2Int(1920, 1080);

        private Slot slotA;
        private Slot slotB;

        private Slot currentSlot;
        private Slot vacantSlot;

        private RenderTexture resultRenderTexture;

        private List<LiveCam> liveCamList;

        private LiveCam currentActiveCamera;

        private bool isOperating;

        public int LiveCamNum => liveCamList.Count;

        public string CurrentLiveCamId => currentActiveCamera.Id;

        private void Start()
        {
            slotA = new Slot(screenResolution.x, screenResolution.y, true);
            slotB = new Slot(screenResolution.x, screenResolution.y, false);

            resultRenderTexture =
                new RenderTexture(screenResolution.x, screenResolution.y, 24, RenderTextureFormat.ARGBFloat)
                    {name = "LiveCamOutput"};
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

            TriggerNextLiveCam(liveCamList[0], LiveCamTriggerMode.CutIn, 0);

            var rig = Instantiate(liveCamRenderRigPrefab);
            rig.SetTexture(resultRenderTexture);
        }

        public async void TriggerNextLiveCam(LiveCam cam, LiveCamTriggerMode mode, float blendingDuration)
        {
            if (!Application.isPlaying)
            {
                Resources.FindObjectsOfTypeAll<LiveCam>().ToList().ForEach(camera => { camera.Deactivate(); });

                cam.Activate();
                return;
            }

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

            isOperating = true;

            if (mode == LiveCamTriggerMode.Blending)
            {
                cam.Activate();

                vacantSlot.SetCamera(cam);

                await UniTask.WaitForEndOfFrame();
                await BlendingCoroutine(currentSlot, vacantSlot, blendingDuration);

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
                blending = currentSlot.Goal;
            }

            isOperating = false;
        }

        public void TriggerNextLiveCamWithIndex(int index, LiveCamTriggerMode mode, float blendingDuration)
        {
            if (index < liveCamList.Count)
            {
                TriggerNextLiveCam(liveCamList[index], mode, blendingDuration);
            }
        }

        public void TriggerNextLiveCamWithId(string id, LiveCamTriggerMode mode, float blendingDuration)
        {
            foreach (var cam in liveCamList)
            {
                if (id == cam.Id)
                {
                    TriggerNextLiveCam(cam, mode, blendingDuration);
                    break;
                }
            }
        }

        private IEnumerator BlendingCoroutine(Slot from, Slot dst, float blendingDuration)
        {
            if (blendingDuration > Mathf.Epsilon)
            {
                var t = 0f;

                while (true)
                {
                    if (t > blendingDuration) break;

                    t += Time.deltaTime;

                    blending = Mathf.Lerp(from.Goal, dst.Goal, t / blendingDuration);

                    yield return null;
                }
            }

            blending = dst.Goal;
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