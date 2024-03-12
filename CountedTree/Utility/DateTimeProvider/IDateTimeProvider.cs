using System;

namespace R8RUtilities
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        void SleepOrSkipTime(long milliseconds);
        void PauseTime();
        void ResumeTime();
    }
}
