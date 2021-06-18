using UnityEngine.Playables;

namespace kodai100.LiveCamCore
{
    public class LiveCamMixerBehaviour : PlayableBehaviour
    {
        private LiveCam prevCam;

        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var trackBinding = playerData as LiveCamController;

            if (!trackBinding)
                return;

            var inputCount = playable.GetInputCount();

            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<LiveCamBehaviour>) playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();

                if (inputWeight > 0)
                {
                    if (prevCam != input.TargetLiveCam)
                    {
                        trackBinding.TriggerNextLiveCam(input.TargetLiveCam, input.TriggerMode, input.BlendingDuration);
                        prevCam = input.TargetLiveCam;
                    }
                }
            }
        }
    }
}