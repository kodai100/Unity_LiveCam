using UnityEngine;

namespace kodai100.LiveCamCore
{
    public class QuaternionLowPassFilter
    {
        private float smoothingFactor;

        public float SmoothingFactor
        {
            get { return smoothingFactor; }
            set { smoothingFactor = value; }
        }

        private Quaternion avg;

        public QuaternionLowPassFilter(float smoothingFactor, Quaternion initialValue)
        {
            this.smoothingFactor = Mathf.Clamp(smoothingFactor, 0.0f, 1.0f);

            if (initialValue == null)
            {
                throw new System.ArgumentNullException("Low Pass Filter initial value cannot be null");
            }

            avg = initialValue;
        }


        public Quaternion Append(Quaternion v)
        {
            var input = v;
            var lavg = avg;
            avg = Quaternion.Lerp(lavg, input, smoothingFactor);

            return avg;
        }
    }
}