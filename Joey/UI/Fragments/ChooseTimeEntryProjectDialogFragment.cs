﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Toggl.Phoebe.Data;
using Toggl.Phoebe.Data.Models;
using Toggl.Joey.UI.Adapters;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace Toggl.Joey.UI.Fragments
{
    public class ChooseTimeEntryProjectDialogFragment : DialogFragment
    {
        private static readonly string TimeEntryIdArgument = "com.toggl.android.time_entry_id";
        private ProjectsAdapter adapter;

        public ChooseTimeEntryProjectDialogFragment (TimeEntryModel model) : base ()
        {
            var args = new Bundle ();
            args.PutString (TimeEntryIdArgument, model.Id.ToString ());

            Arguments = args;
        }

        public ChooseTimeEntryProjectDialogFragment (IntPtr jref, Android.Runtime.JniHandleOwnership xfer) : base (jref, xfer)
        {
        }

        private Guid TimeEntryId {
            get {
                var id = Guid.Empty;
                if (Arguments != null) {
                    Guid.TryParse (Arguments.GetString (TimeEntryIdArgument), out id);
                }
                return id;
            }
        }

        private TimeEntryModel model;

        public override void OnCreate (Bundle state)
        {
            base.OnCreate (state);

            model = Model.ById<TimeEntryModel> (TimeEntryId);
            if (model == null) {
                Dismiss ();
            }
        }

        public override Dialog OnCreateDialog (Bundle state)
        {
            adapter = new ProjectsAdapter ();
            return new AlertDialog.Builder (Activity)
                .SetTitle (Resource.String.ChooseTimeEntryProjectDialogTitle)
                    .SetAdapter (adapter, OnItemSelected)
                .SetNegativeButton (Resource.String.ChooseTimeEntryProjectDialogCancel, OnCancelButtonClicked)
                .Create ();
        }

        private void OnCancelButtonClicked (object sender, DialogClickEventArgs args)
        {
            Dismiss ();
        }

        private void OnItemSelected (object sender, DialogClickEventArgs args)
        {
            if (model != null) {
                var m = adapter.GetModel (args.Which);
                var task = m as TaskModel;
                var project = task != null ? task.Project : m as ProjectModel;

                if (project != null || task != null) {
                    model.Project = project;
                    model.Task = task;
                }
            }

            Dismiss ();
        }
    }
}
