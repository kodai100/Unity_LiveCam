using UnityEngine;
using System.Collections;

namespace kodai100.LiveCamCore.Splines
{
    public class Spline : ScriptableObject
    {
        public const int GIZMO_SMOOTH_LEVEL = 10;
        public const float NORMALIZE_ANGLE = 1f / 360;

        public ControlPoint[] cps;

        public Vector3 Position(float t)
        {
            return CatmullSplineUtil.Position(t, GetCP);
        }

        public Vector3 Velosity(float t)
        {
            return CatmullSplineUtil.Velosity(t, GetCP);
        }

        public float Duration()
        {
            if (cps == null || cps.Length < 2)
                return 0f;
            return float.MaxValue;
        }

        public float Period()
        {
            return (cps == null || cps.Length < 2) ? 0f : cps.Length;
        }

        public Vector3 GetCP(int i)
        {
            while (i < 0)
                i += cps.Length;
            i = i % cps.Length;
            return cps[i].position;
        }

        [System.Serializable]
        public class ControlPoint
        {
            public Vector3 position;
        }
    }
}