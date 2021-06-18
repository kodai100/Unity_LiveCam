using UnityEngine;

namespace kodai100.LiveCamCore
{
    public abstract class TargetSystem : MonoBehaviour
    {
        public abstract Vector3 Position { get; protected set; }
    }
}