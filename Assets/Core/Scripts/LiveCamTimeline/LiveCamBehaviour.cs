using System;
using UnityEngine.Playables;

namespace kodai100.LiveCamCore
{
    [Serializable]
    public class LiveCamBehaviour : PlayableBehaviour
    {
        public LiveCamTriggerMode TriggerMode = LiveCamTriggerMode.CutIn;
        public LiveCam TargetLiveCam;

        public override void OnPlayableCreate(Playable playable)
        {
        }
    }
}