using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace kodai100.LiveCamCore
{
    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class LiveCam : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        [Space] [SerializeField] private TargetSystem target;
        [SerializeField] private Rail rail;

        [Space] [SerializeField] protected EaseType easeType = EaseType.InOutSine;
        [SerializeField] private bool resetFractionOnTrigger;
        [SerializeField, Range(0, 1)] private float fraction;
        [SerializeField] private float fractionSpeed = 1;
        [SerializeField] private bool fractionPingPong = true;
        [SerializeField] private bool fractionEndless = false;
        [SerializeField] private float loopInterval = 10;

        private Coroutine coroutine;
        private float time;
        private bool direction = true;

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
                // TODO: add jitter transform
            }

            if (target)
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

                        direction = true;
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