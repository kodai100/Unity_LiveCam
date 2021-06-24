using UnityEngine;

namespace kodai100.LiveCamCore
{
    public abstract class TargetSystem : MonoBehaviour
    {
        [SerializeField] protected bool useWiggle;
        [SerializeField] protected Wiggler wiggler;
        public abstract Vector3 Position { get; protected set; }
    }
}