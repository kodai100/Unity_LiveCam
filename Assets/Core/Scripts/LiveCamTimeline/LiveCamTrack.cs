using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace kodai100.LiveCamCore
{
    [TrackColor(1f, 0.4f, 0.4f)]
    [TrackClipType(typeof(LiveCamClip))]
    [TrackBindingType(typeof(LiveCamController))]
    public class LiveCamTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<LiveCamMixerBehaviour>.Create(graph, inputCount);
        }
    }
}