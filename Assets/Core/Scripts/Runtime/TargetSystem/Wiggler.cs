using System;
using UnityEngine;

namespace kodai100.LiveCamCore
{
    [Serializable]
    public class Wiggler
    {
        public Vector3 MoveRange = Vector3.one * 0.2f;
        public Vector3 NoiseSeed = Vector3.one * 2.5f;
        public Vector3 Roughness = Vector3.one * 0.5f;

        public float Speed = 0.5f;

        private float time;

        private Vector3 cache = Vector3.zero;

        public Vector3 GetWiggle()
        {
            time += Time.deltaTime;
            cache.x = MoveRange.x * Mathf.PerlinNoise(NoiseSeed.x + time * Speed, NoiseSeed.y) * Roughness.x;
            cache.y = MoveRange.y * Mathf.PerlinNoise(NoiseSeed.x, NoiseSeed.y + time * Speed) * Roughness.y;
            cache.z = MoveRange.z * Mathf.PerlinNoise(NoiseSeed.y, NoiseSeed.z + time * Speed) * Roughness.z;
            return cache;
        }
    }
}