using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class FixedTargetSystem : TargetSystem
    {
        public override Vector3 Position
        {
            get => transform.position;
            protected set { }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }
}