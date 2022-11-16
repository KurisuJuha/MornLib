﻿using UnityEngine;
namespace MornLib.Cores {
    [System.Serializable]
    public struct MornFloatRange {
        public float Start;
        public float End;
        public MornFloatRange(float start,float end) {
            Start = start;
            End   = end;
        }
        public float Lerp(float rate) => Mathf.Lerp(Start,End,Mathf.Clamp01(rate));
        public float Clamp(float value) => Mathf.Clamp(value,Start,End);
        public float Random() => UnityEngine.Random.Range(Start,End);
    }
}
