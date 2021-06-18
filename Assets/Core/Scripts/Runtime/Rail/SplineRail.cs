using kodai100.LiveCamCore.Splines;
using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class SplineRail : Rail
    {
        [SerializeField] private Spline spline;

        public override Vector3 GetCurrentPosition => spline.Position(Fraction * spline.Period());

        public override float Fraction { get; set; } = 0f;

        public override Vector3 Tangent => spline.Velosity(Fraction * spline.Period()).normalized;


        public const int GIZMO_SMOOTH_LEVEL = 10;

        public const float JET_K_MIN = 0.01f;
        public const float JET_K_MAX = 0.1f;

        private void OnDrawGizmos()
        {
            if (spline == null) return;

            var cps = spline.cps;
            System.Func<int, Vector3> GetCP = spline.GetCP;

            if (cps == null || cps.Length < 2)
                return;

            var dt = 1f / GIZMO_SMOOTH_LEVEL;
            var kMin = float.MaxValue;
            var kMax = 0f;
            for (var i = 0; i < cps.Length; i++)
            {
                var t = (float) i;
                for (var j = 0; j < GIZMO_SMOOTH_LEVEL; j++)
                {
                    var k = CatmullSplineUtil.Curvature(t, spline.GetCP);
                    k = Mathf.Clamp(k, JET_K_MIN, JET_K_MAX);
                    if (k < kMin)
                        kMin = k;
                    else if (kMax < k)
                        kMax = k;
                    t += dt;
                }
            }

            var jetA = 0.66666f / (kMin - kMax);
            var jetB = -jetA * kMax;
            var startPos = CatmullSplineUtil.Position(0f, GetCP);
            for (var i = 0; i < cps.Length; i++)
            {
                var t = (float) i;
                for (var j = 0; j < GIZMO_SMOOTH_LEVEL; j++)
                {
                    var k = CatmullSplineUtil.Curvature(t, spline.GetCP);
                    k = Mathf.Clamp(k, kMin, kMax);
                    Gizmos.color = Color.HSVToRGB(jetA * k + jetB, 1f, 1f);

                    var endPos = CatmullSplineUtil.Position(t += dt, GetCP);
                    Gizmos.DrawLine(startPos, endPos);
                    startPos = endPos;
                }
            }
        }
    }
}