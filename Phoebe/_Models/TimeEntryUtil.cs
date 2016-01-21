﻿using System;
using System.Collections.Generic;
using System.Linq;
using Toggl.Phoebe.Data;
using Toggl.Phoebe.Data.DataObjects;
using Toggl.Phoebe.Data.Json;

namespace Toggl.Phoebe.Models
{
    // Empty interface just to hide references to IDiffComparable
    public interface IHolder : IDiffComparable
    {
    }

    public interface ITimeEntryHolder : IHolder
    {
        TimeEntryData Data { get; }
        IList<TimeEntryData> DataCollection { get; }
        TimeEntryInfo Info { get; }
        IList<string> Guids { get; }

        TimeSpan GetDuration ();
        DateTime GetStartTime ();
    }

    public enum TimeEntryGroupMethod {
        Single,
        ByDateAndTask
    }

    public class TimeEntryGrouper
    {
        readonly TimeEntryGroupMethod Method;

        public TimeEntryGrouper (TimeEntryGroupMethod method)
        {
            Method = method;
        }

        public IEnumerable<ITimeEntryHolder> Group (IEnumerable<TimeEntryHolder> items)
        {
            return Method == TimeEntryGroupMethod.Single
                   ? items.Cast<ITimeEntryHolder> () : TimeEntryGroup.Group (items);
        }

        public IEnumerable<TimeEntryHolder> Ungroup (IEnumerable<ITimeEntryHolder> groups)
        {
            return Method == TimeEntryGroupMethod.Single
                   ? groups.Cast<TimeEntryHolder> () : TimeEntryGroup.Ungroup (groups.Cast<TimeEntryGroup> ());
        }
    }

    public class TimeEntryMsg : List<Tuple<DataVerb, TimeEntryData>>, IDataSyncGroupMsg<TimeEntryData>
    {
        public DataDir Dir { get; private set; }
        public Type DataType { get { return typeof(TimeEntryData); } }

        public IEnumerable<DataSyncMsg<TimeEntryData>> Messages {
            get { 
                return this.Select (x => new DataSyncMsg<TimeEntryData> (Dir, x.Item1, x.Item2));
            }
        }

        public IEnumerable<IDataSyncMsg> RawMessages {
            get { 
                return Messages;
            }
        }

        public TimeEntryMsg (DataDir dir, IEnumerable<Tuple<DataVerb, TimeEntryData>> msgs)
            : base (msgs)
        {
            Dir = dir;
        }

        public TimeEntryMsg (DataDir dir, DataVerb action, TimeEntryData data)
            : base (new [] { Tuple.Create (action, data) })
        {
            Dir = dir;
        }
    }
}
