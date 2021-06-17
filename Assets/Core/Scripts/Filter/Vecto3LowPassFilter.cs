using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class Vector3LowPassFilter
    {
        private float smoothingFactor;

        public float SmoothingFactor
        {
            get { return smoothingFactor; }
            set { smoothingFactor = value; }
        }

        private Vector3 avg;

        public Vector3LowPassFilter(float smoothingFactor, Vector3 initialValue)
        {
            this.smoothingFactor = Mathf.Clamp(smoothingFactor, 0.0f, 1.0f);

            if (initialValue == null)
            {
                throw new System.ArgumentNullException("Low Pass Filter initial value cannot be null");
            }

            avg = initialValue;
        }


        public Vector3 Append(Vector3 v)
        {
            var input = v;
            var lavg = avg;
            avg = Vector3.Lerp(lavg, input, smoothingFactor);

            return avg;
        }
    }
}