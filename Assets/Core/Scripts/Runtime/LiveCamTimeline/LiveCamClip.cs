using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace kodai100.LiveCamCore
{
    [Serializable]
    public class LiveCamClip : PlayableAsset, ITimelineClipAsset
    {
        private LiveCamBehaviour template = new LiveCamBehaviour();

        public LiveCamTriggerMode TriggerMode = LiveCamTriggerMode.CutIn;
        public float BlendingDuration = 1f;
        public ExposedReference<LiveCam> TargetLiveCam;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<LiveCamBehaviour>.Create(graph, template);
            var clone = playable.GetBehaviour();
            clone.TargetLiveCam = TargetLiveCam.Resolve(graph.GetResolver());
            clone.BlendingDuration = BlendingDuration;
            clone.TriggerMode = TriggerMode;
            return playable;
        }
    }
}