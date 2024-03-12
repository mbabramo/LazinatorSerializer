using System;

namespace R8RUtilities
{
    public class AbsoluteFakeDateTimeProvider : IDateTimeProvider
    {
        public DateTime currentTime = new DateTime(2000, 1, 1);
        public TimeSpan timeToAddWithEachRequest = new TimeSpan(0, 1, 0);
        bool isPaused = false;

        public DateTime Now
        {
            get
            {
                if (!isPaused)
                    currentTime += timeToAddWithEachRequest;
                return currentTime;
            }
        }

        public void SleepOrSkipTime(long milliseconds)
        {
            currentTime += TimeSpan.FromMilliseconds(milliseconds);
        }

        public void PauseTime()
        {
            isPaused = true;
        }
        public void ResumeTime()
        {
            isPaused = false;
        }
    }
}
