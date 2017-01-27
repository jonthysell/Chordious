// 
// ChordFinder.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
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
using System.Threading;
using System.Threading.Tasks;

namespace com.jonthysell.Chordious.Core
{
    public class ChordFinder
    {
        public static ChordFinderResultSet FindChords(IChordFinderOptions chordFinderOptions)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task<ChordFinderResultSet> task = FindChordsAsync(chordFinderOptions, cts.Token);
            task.Wait();

            return task.Result;
        }

        public static async Task<ChordFinderResultSet> FindChordsAsync(IChordFinderOptions chordFinderOptions, CancellationToken cancelToken)
        {
            if (null == chordFinderOptions)
            {
                throw new ArgumentNullException("chordFinderOptions");
            }

            InternalNote root = NoteUtils.ToInternalNote(chordFinderOptions.RootNote);
            IChordQuality chordQuality = chordFinderOptions.ChordQuality;

            InternalNote[] notesInChord = NamedInterval.GetNotes(root, chordQuality.Intervals);

            ChordFinderResultSet results = new ChordFinderResultSet(chordFinderOptions);

            if (cancelToken.IsCancellationRequested)
            {
                return results;
            }

            await FindAllChordsAsync(results, null, notesInChord, 0, chordFinderOptions, cancelToken);

            return results;
        }

        private static async Task FindAllChordsAsync(ChordFinderResultSet results, NoteNode noteNode, InternalNote[] targetNotes, int str, IChordFinderOptions chordFinderOptions, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return;
            }

            IInstrument instrument = chordFinderOptions.Instrument;

            if (str == instrument.NumStrings) // Build a chord result
            {
                int[] marks = new int[instrument.NumStrings];

                NoteNode nn = noteNode;
                str--;

                bool[] hasNotes = new bool[targetNotes.Length];

                // Walk back up the tree to set the marks on the result and flag each target note
                while (nn != null)
                {
                    marks[str] = nn.Fret;

                    for (int i = 0; i < targetNotes.Length; i++)
                    {
                        if (nn.Note == targetNotes[i])
                        {
                            hasNotes[i] = true;
                        }
                    }

                    nn = nn.Parent;
                    str--;
                }

                InternalNote rootNote = NoteUtils.ToInternalNote(chordFinderOptions.RootNote);

                // Add result if it had all the target notes
                bool valid = true;
                for (int i = 0; i < hasNotes.Length; i++)
                {
                    // Validate:
                    // 1. All notes when !AllowRootlessChords
                    // 2. Non-root notes only when AllowRootlessChords
                    if (!chordFinderOptions.AllowRootlessChords ||
                       (chordFinderOptions.AllowRootlessChords && targetNotes[i] != rootNote))
                    {
                        valid = valid && hasNotes[i];
                    }
                }

                if (valid)
                {
                    results.AddResult(marks);
                }
            }
            else // Keep building the tree
            {
                // Look at the muted string
                if (chordFinderOptions.AllowMutedStrings)
                {
                    NoteNode muted = new NoteNode();
                    muted.Parent = noteNode;
                    await FindAllChordsAsync(results, muted, targetNotes, str + 1, chordFinderOptions, cancelToken);
                }

                // Look at all the notes on the string
                int startingFret = chordFinderOptions.AllowOpenStrings ? 0 : 1;
                for (int fret = startingFret; fret <= chordFinderOptions.MaxFret; fret++)
                {
                    InternalNote note = NoteUtils.Shift(chordFinderOptions.Tuning.RootNotes[str].InternalNote, fret);

                    // See if the note is a target note
                    for (int i = 0; i < targetNotes.Length; i++)
                    {
                        // If it's a target note add it and search on the next string
                        if (note == targetNotes[i])
                        {
                            NoteNode child = new NoteNode(fret, note, noteNode);
                            await FindAllChordsAsync(results, child, targetNotes, str + 1, chordFinderOptions, cancelToken);
                            break;
                        }
                    }
                }
            }
        }

        protected class NoteNode
        {
            public int Fret { get; set; }
            public InternalNote? Note { get; set; }
            public NoteNode Parent { get; set; }

            public NoteNode(int fret, InternalNote? note, NoteNode parent)
            {
                Fret = fret;
                Note = note;
                Parent = parent;
            }

            public NoteNode() : this(-1, null, null) {}

            public NoteNode(NoteNode parent) : this(-1, null, parent) {}
        }
    }
}