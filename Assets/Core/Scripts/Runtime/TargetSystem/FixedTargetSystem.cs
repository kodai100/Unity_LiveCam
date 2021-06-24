using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class FixedTargetSystem : TargetSystem
    {
        public override Vector3 Position { get; protected set; }

        public void Update()
        {
            Position = useWiggle ? transform.position + wiggler.GetWiggle() : transform.position;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Position, 0.2f);
        }
    }
}