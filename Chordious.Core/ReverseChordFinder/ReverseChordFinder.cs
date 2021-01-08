// 
// ReverseChordFinder.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2019,  Jon Thysell <http://jonthysell.com>
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chordious.Core
{
    public class ReverseChordFinder
    {
        public static ReverseChordFinderResultSet FindChords(IReverseChordFinderOptions reverseChordFinderOptions)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task<ReverseChordFinderResultSet> task = FindChordsAsync(reverseChordFinderOptions, cts.Token);
            task.Wait();

            return task.Result;
        }

        public static async Task<ReverseChordFinderResultSet> FindChordsAsync(IReverseChordFinderOptions reverseChordFinderOptions, CancellationToken cancelToken)
        {
            if (null == reverseChordFinderOptions)
            {
                throw new ArgumentNullException(nameof(reverseChordFinderOptions));
            }

            ReverseChordFinderResultSet results = new ReverseChordFinderResultSet(reverseChordFinderOptions);

            if (cancelToken.IsCancellationRequested)
            {
                return results;
            }

            int[] marks = reverseChordFinderOptions.Marks;

            HashSet<InternalNote> inputNotes = new HashSet<InternalNote>();

            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i] >= 0)
                {
                    inputNotes.Add(NoteUtils.Shift(reverseChordFinderOptions.Tuning.RootNotes[i].InternalNote, marks[i]));
                }
            }

            foreach (InternalNote rootNote in NoteUtils.InternalNotes)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return results;
                }

                foreach (IChordQuality chordQuality in reverseChordFinderOptions.ChordQualities)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        return results;
                    }

                    InternalNote[] notesInChord = NamedInterval.GetNotes(rootNote, chordQuality.Intervals);

                    if (inputNotes.SetEquals(notesInChord))
                    {
                        results.AddResult(rootNote, chordQuality);
                    }

                    await Task.Yield();
                }

                await Task.Yield();
            }

            return results;
        }
    }
}
