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

    public class LiveCamController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float blending = 0f;

        [SerializeField] private LiveCamRenderRig liveCamRenderRigPrefab;

        [SerializeField] private Material blendingMaterial;

        [SerializeField] private float blendingTime = 1f;

        private RenderTexture standbyRenderTexture;
        private RenderTexture activeRenderTexture;

        private RenderTexture resultRenderTexture;

        private List<LiveCam> liveCamList;

        private LiveCam currentActiveCamera;

        private bool isOperating;

        public int LiveCamNum => liveCamList.Count;
        public int CurrentCameraIndex { get; private set; }

        private void Start()
        {
            standbyRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBHalf);
            standbyRenderTexture.Create();

            activeRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBHalf);
            activeRenderTexture.Create();

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

            TriggerNextLiveCam(liveCamList[0], LiveCamTriggerMode.CutIn);
            CurrentCameraIndex = 0;

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
                // swap rendering target and blending value
                currentActiveCamera.SetTexture(standbyRenderTexture);

                cam.SetTexture(activeRenderTexture);
                cam.Activate();
                blending = 0;

                await BlendingCoroutine();

                // wait until blending end
                currentActiveCamera.Deactivate();
                currentActiveCamera = cam;
            }
            else
            {
                if (currentActiveCamera == null)
                {
                    currentActiveCamera = cam;
                    currentActiveCamera.SetTexture(activeRenderTexture);
                    currentActiveCamera.Activate();
                }
                else
                {
                    currentActiveCamera.Deactivate();
                    currentActiveCamera = cam;
                    currentActiveCamera.SetTexture(activeRenderTexture);
                    currentActiveCamera.Activate();
                }

                blending = 1;
            }
        }

        public void TriggerNextLiveCamWithIndex(int index, LiveCamTriggerMode mode)
        {
            if (index < liveCamList.Count)
            {
                TriggerNextLiveCam(liveCamList[index], mode);
            }
        }

        private IEnumerator BlendingCoroutine()
        {
            isOperating = true;


            var t = 0f;
            while (true)
            {
                if (t > blendingTime) break;
                t += Time.deltaTime;

                blending = t / blendingTime;

                yield return null;
            }

            blending = 1f;
            isOperating = false;
        }

        private void Update()
        {
            blendingMaterial.SetFloat("_Blending", blending);
            blendingMaterial.SetTexture("_SrcOne", standbyRenderTexture);
            blendingMaterial.SetTexture("_SrcTwo", activeRenderTexture);
            Graphics.Blit(null, resultRenderTexture, blendingMaterial);
        }
    }
}