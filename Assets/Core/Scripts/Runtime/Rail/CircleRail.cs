using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class CircleRail : Rail
    {
        [SerializeField] private Transform center;
        [SerializeField] private float radius = 10;

        [SerializeField] private float startAngle = 0f;
        [SerializeField] private float endAngle = 180f;

        public override Vector3 GetCurrentPosition => Calc();
        public override float Fraction { get; set; } = 0f;

        public override Vector3 Tangent => CalcTangent();


        private Vector3 Calc()
        {
            if (!center) return Vector3.zero;

            var radian = (endAngle - startAngle) * Mathf.Deg2Rad * Fraction + startAngle * Mathf.Deg2Rad;
            var nonRotatedPos = new Vector3(radius * Mathf.Cos(radian), 0, radius * Mathf.Sin(radian));
            var rotatedPos = center.rotation * nonRotatedPos;
            return rotatedPos + center.position;
        }

        private Vector3 CalcTangent()
        {
            if (!center) return Vector3.zero;

            var radian = (endAngle - startAngle) * Mathf.Deg2Rad * Fraction + startAngle * Mathf.Deg2Rad;
            var nonRotatedPos = new Vector3(-Mathf.Sin(radian), 0, Mathf.Cos(radian));
            return center.rotation * nonRotatedPos;
        }

        private void OnDrawGizmos()
        {
            if (center == null) return;

            Gizmos.color = Color.red;
            if (endAngle > startAngle)
            {
                GizmosExtensions.DrawWireOpenArc(center.position, radius, startAngle, endAngle, 20, center.rotation);
            }
        }
    }
}