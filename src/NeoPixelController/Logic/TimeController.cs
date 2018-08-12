using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace NeoPixelController.Logic
{
    public class TimeController
    {
        private EffectTime time = new EffectTime();
        private Stopwatch stopwatch = new Stopwatch();

        private float _FramePerSecond = 60;
        public float FramePerSecond
        {
            get
            {
                return _FramePerSecond;
            }
            set
            {
                if (value != _FramePerSecond)
                {
                    _FramePerSecond = value;
                    TimeBetweenFrames = (int)(1.0f / FramePerSecond);
                }
            }
        }

        private int TimeBetweenFrames = 16;

        public async Task<EffectTime> UpdateTime()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
                time.DeltaTime = TimeBetweenFrames;
                time.Time = 0;
                return time;
            }
            await DoWait();

            var eclipsedMilliseconds = stopwatch.ElapsedMilliseconds;
            //eclipsedMilliseconds = Math.Min(eclipsedMilliseconds - time.Time, time.Time + 16);
            time.DeltaTime = eclipsedMilliseconds - time.Time;
            time.Time = eclipsedMilliseconds;
            return time;
        }

        private async Task DoWait()
        {
            var timeUsed = stopwatch.ElapsedMilliseconds - time.Time;
            var sleepTime = Math.Max(0, TimeBetweenFrames - timeUsed);
            await Task.Delay((int)TimeBetweenFrames);
        }

    }
}
