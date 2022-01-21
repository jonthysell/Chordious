// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ScaleEditorViewModel : NamedIntervalEditorViewModel
    {
        public override string Title
        {
            get
            {
                return IsNew ? Strings.ScaleEditorNewTitle : (ReadOnly ? Strings.ScaleEditorEditReadOnlyTitle : Strings.ScaleEditorEditTitle);
            }
        }

        public override string NameToolTip
        {
            get
            {
                return Strings.ScaleEditorNameToolTip;
            }
        }

        public override string IntervalsToolTip
        {
            get
            {
                return Strings.ScaleEditorIntervalsToolTip;
            }
        }

        public override string ExampleToolTip
        {
            get
            {
                return Strings.ScaleEditorExampleToolTip;
            }
        }

        private readonly Action<string, int[]> _callback;

        public ScaleEditorViewModel(Action<string, int[]> callback) : base()
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public ScaleEditorViewModel(string name, int[] intervals, bool readOnly, Action<string, int[]> callback) : base(name, intervals, readOnly)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        protected override void OnAccept()
        {
            _callback(Name, GetIntervalArray());
        }
    }
}
