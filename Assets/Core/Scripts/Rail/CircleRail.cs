using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class CircleRail : Rail
    {
        [SerializeField] private Transform center;
        [SerializeField] private float radius = 10;

        [SerializeField] private float startPI = 1f;
        [SerializeField] private float endPI = 2f;

        public override Vector3 GetCurrentPosition => Calc();
        public override float Fraction { get; set; } = 0f;


        private Vector3 Calc()
        {
            if (!center) return Vector3.zero;

            var radian = (endPI - startPI) * Fraction * Mathf.PI + startPI * Mathf.PI;
            var nonRotatedPos = new Vector3(radius * Mathf.Cos(radian), 0, radius * Mathf.Sin(radian));
            var rotatedPos = center.rotation * nonRotatedPos;
            return rotatedPos + center.position;
        }

        private void OnDrawGizmos()
        {
            if (center == null) return;

            Gizmos.color = Color.red;
            GizmosExtensions.DrawWireCircle(center.position, radius, center.rotation);
        }
    }
}