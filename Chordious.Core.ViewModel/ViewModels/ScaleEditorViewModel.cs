// 
// ScaleEditorViewModel.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

using com.jonthysell.Chordious.Core.ViewModel.Resources;

namespace com.jonthysell.Chordious.Core.ViewModel
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

        private Action<string, int[]> _callback;

        public ScaleEditorViewModel(Action<string, int[]> callback) : base()
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            _callback = callback;
        }

        public ScaleEditorViewModel(string name, int[] intervals, bool readOnly, Action<string, int[]> callback) : base(name, intervals, readOnly)
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            _callback = callback;
        }

        protected override void OnAccept()
        {
            _callback(Name, GetIntervalArray());
        }
    }
}
