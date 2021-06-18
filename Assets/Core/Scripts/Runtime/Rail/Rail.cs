using UnityEngine;

namespace kodai100.LiveCamCore
{
    public abstract class Rail : MonoBehaviour
    {
        public abstract Vector3 GetCurrentPosition { get; }
        public abstract float Fraction { get; set; }
        public abstract Vector3 Tangent { get; }
    }
}