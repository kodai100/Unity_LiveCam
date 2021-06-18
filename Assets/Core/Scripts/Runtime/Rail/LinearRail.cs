using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class LinearRail : Rail
    {
        [SerializeField] private Transform point1;
        [SerializeField] private Transform point2;

        public override Vector3 GetCurrentPosition => Vector3.Lerp(point1.position, point2.position, Fraction);

        public override Vector3 Tangent => (point2.position - point1.position).normalized;

        public override float Fraction { get; set; } = 0f;

        private void OnDrawGizmos()
        {
            if (point1 != null && point2 != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(point1.position, point2.position);
            }
        }
    }
}