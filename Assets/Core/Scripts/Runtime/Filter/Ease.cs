using UnityEngine;

namespace kodai100.LiveCamCore
{
    public enum EaseType
    {
        InOutSine,
        InOutCubic,
        Linear,
        OutSine
    }

    public static class EaseUtility
    {
        public static float Ease(float input, EaseType easeType)
        {
            return easeType switch
            {
                EaseType.InOutSine => EaseInOutSin(input),
                EaseType.InOutCubic => EaseInOutCubic(input),
                EaseType.Linear => input,
                EaseType.OutSine => EaseOutSine(input),
                _ => input
            };
        }

        private static float EaseInOutSin(float value)
        {
            return -(Mathf.Cos(Mathf.PI * value) - 1) / 2;
        }

        private static float EaseInOutCubic(float value)
        {
            return value < 0.5f ? 4f * value * value * value : 1f - Mathf.Pow(-2f * value + 2f, 3f) / 2f;
        }

        private static float EaseOutSine(float value)
        {
            return Mathf.Sin((value * Mathf.PI) / 2);
        }
    }
}