using UnityEngine;

namespace kodai100.LiveCamCore
{
    [ExecuteInEditMode]
    public class SpatialTargetSystem : TargetSystem
    {
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private Transform target;

        [SerializeField] private float smoothingFactor = 0.05f;

        public override Vector3 Position { get; protected set; }

        private Vector3LowPassFilter filter;

        public void Update()
        {
            if (!target) return;

            var position = useWiggle ? target.position + offset + wiggler.GetWiggle() : target.position + offset;

            filter ??= new Vector3LowPassFilter(smoothingFactor, position);

            Position = Application.isPlaying
                ? filter.Append(position)
                : position;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Position, 0.2f);
        }
    }
}