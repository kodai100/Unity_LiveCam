using UnityEngine;

namespace kodai100.LiveCamCore
{
    [ExecuteInEditMode]
    public class ActorTargetSystem : TargetSystem
    {
        [SerializeField] private float fixedHeight = 1.3f;
        [SerializeField] private Transform targetHip;

        [SerializeField] private float smoothingFactor = 0.05f;

        public override Vector3 Position { get; protected set; }

        private Vector3LowPassFilter filter;

        public void Update()
        {
            if (!targetHip) return;

            var position = targetHip.position;

            filter ??= new Vector3LowPassFilter(smoothingFactor, new Vector3(position.x, fixedHeight, position.z));

            Position = Application.isPlaying
                ? filter.Append(new Vector3(position.x, fixedHeight, position.z))
                : new Vector3(position.x, fixedHeight, position.z);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Position, 0.2f);
        }
    }
}