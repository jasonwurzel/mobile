using System;

namespace Toggl.Phoebe.Analytics
{
    public interface ITracker
    {
        string CurrentScreen { set; }
        string RunningExperiment { set; }
        PlanType UserPlan { set; }
        void SendTiming (TimedEvent timedEvent, TimeSpan duration, string label=null);
    }
}
