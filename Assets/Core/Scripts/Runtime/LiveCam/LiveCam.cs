using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace kodai100.LiveCamCore
{
    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class LiveCam : MonoBehaviour
    {
        [SerializeField] private string id = "/CAM";

        [SerializeField] private Camera camera;

        [Space] [SerializeField] private bool lookTangentDirection;

        [Space] [SerializeField] private TargetSystem target;
        [SerializeField] private Rail rail;

        [Space] [SerializeField] protected EaseType easeType = EaseType.InOutSine;
        [SerializeField] private bool resetFractionOnTrigger;
        [SerializeField] private bool initialDirection = true;
        [SerializeField, Range(0, 1)] private float fraction;
        [SerializeField] private float fractionSpeed = 1;
        [SerializeField] private bool fractionPingPong = true;
        [SerializeField] private bool fractionEndless = false;
        [SerializeField] private float loopInterval = 10;

        private Coroutine coroutine;
        private float time;
        private bool direction;

        public string Id => id;

        public void SetTexture(RenderTexture texture)
        {
            camera.targetTexture = texture;
        }

        private void Reset()
        {
            camera = GetComponent<Camera>();
        }

        private void Start()
        {
            gameObject.name = id;

            direction = initialDirection;

            if (Application.isPlaying)
            {
                coroutine = StartCoroutine(Loop());
            }
        }

        private void Update()
        {
            if (rail)
            {
                rail.Fraction = EaseUtility.Ease(fraction, easeType);
                transform.position = rail.GetCurrentPosition;

                if (lookTangentDirection)
                {
                    transform.forward = direction ? rail.Tangent : -rail.Tangent;
                }
            }

            
        }

        private void LateUpdate()
        {
            if (target != null && !lookTangentDirection)
            {
                var forwardToTarget = target.Position - transform.position;
                var rot = Quaternion.LookRotation(forwardToTarget);

                transform.rotation = rot;
            }
        }

        private IEnumerator Loop()
        {
            while (true)
            {
                time += Time.deltaTime;

                var frac = direction
                    ? (time * fractionSpeed) / loopInterval
                    : 1f - (time * fractionSpeed) / loopInterval;

                fraction = Mathf.Max(Mathf.Min(frac, 1f), 0f);

                if (time * fractionSpeed > loopInterval)
                {
                    if (fractionPingPong)
                    {
                        time = 0;
                        direction = !direction;
                    }
                    else
                    {
                        if (fractionEndless)
                        {
                            time = 0;
                        }

                        direction = initialDirection;
                    }
                }

                yield return null;
            }
        }


        public void Activate()
        {
            camera.enabled = true;

            if (resetFractionOnTrigger)
            {
                time = 0;
            }
        }

        public void Deactivate()
        {
            camera.enabled = false;
        }

        private void OnDestroy()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }
}