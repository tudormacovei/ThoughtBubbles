using UnityEngine;

namespace tdk.Systems
{
    /// <summary>
    /// Timer that counts up from zero to infinity.
    /// </summary>
    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }

        public override void Tick()
        {
            if (IsRunning)
            {
                CurrentTime += Time.deltaTime;
            }
        }

        public override bool IsFinished => false;
    }
}