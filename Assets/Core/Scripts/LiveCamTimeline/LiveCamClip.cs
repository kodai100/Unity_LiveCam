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
        public ExposedReference<LiveCam> TargetLiveCam;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<LiveCamBehaviour>.Create(graph, template);
            var clone = playable.GetBehaviour();
            clone.TargetLiveCam = TargetLiveCam.Resolve(graph.GetResolver());
            clone.TriggerMode = TriggerMode;
            return playable;
        }
    }
}