// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core.ViewModel.Resources;

namespace Chordious.Core.ViewModel
{
    public class ChordQualityEditorViewModel : NamedIntervalEditorViewModel
    {
        public override string Title
        {
            get
            {
                return IsNew ? Strings.ChordQualityEditorNewTitle : (ReadOnly ? Strings.ChordQualityEditorEditReadOnlyTitle : Strings.ChordQualityEditorEditTitle);
            }
        }

        public override string NameToolTip
        {
            get
            {
                return Strings.ChordQualityEditorNameToolTip;
            }
        }

        public override string IntervalsToolTip
        {
            get
            {
                return Strings.ChordQualityEditorIntervalsToolTip;
            }
        }

        public override string ExampleToolTip
        {
            get
            {
                return Strings.ChordQualityEditorExampleToolTip;
            }
        }

        public string AbbreviationLabel
        {
            get
            {
                return Strings.ChordQualityEditorAbbreviationLabel;
            }
        }

        public string AbbreviationToolTip
        {
            get
            {
                return Strings.ChordQualityEditorAbbreviationToolTip;
            }
        }

        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                _abbreviation = value;
                OnPropertyChanged(nameof(Abbreviation));
                Accept.NotifyCanExecuteChanged();
            }
        }
        private string _abbreviation;

        private readonly Action<string, string, int[]> _callback;

        public ChordQualityEditorViewModel(Action<string, string, int[]> callback) : base()
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public ChordQualityEditorViewModel(string name, string abbreviation, int[] intervals, bool readOnly, Action<string, string, int[]> callback) : base(name, intervals, readOnly)
        {
            _abbreviation = abbreviation;
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        protected override void OnAccept()
        {
            _callback(Name, Abbreviation, GetIntervalArray());
        }
    }
}
