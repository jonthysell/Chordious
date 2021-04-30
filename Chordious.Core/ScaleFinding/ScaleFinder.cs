// 
// ScaleFinder.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017, 2019, 2021 Jon Thysell <http://jonthysell.com>
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
    public class ScaleFinder
    {
        public static ScaleFinderResultSet FindScales(IScaleFinderOptions scaleFinderOptions)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task<ScaleFinderResultSet> task = FindScalesAsync(scaleFinderOptions, cts.Token);
            task.Wait();

            return task.Result;
        }

        public static async Task<ScaleFinderResultSet> FindScalesAsync(IScaleFinderOptions scaleFinderOptions, CancellationToken cancelToken)
        {
            if (null == scaleFinderOptions)
            {
                throw new ArgumentNullException(nameof(scaleFinderOptions));
            }

            InternalNote root = NoteUtils.ToInternalNote(scaleFinderOptions.RootNote);
            IScale scale = scaleFinderOptions.Scale;

            InternalNote[] notesInScale = NamedInterval.GetNotes(root, scale.Intervals);

            ScaleFinderResultSet results = new ScaleFinderResultSet(scaleFinderOptions);

            if (cancelToken.IsCancellationRequested)
            {
                return results;
            }

            foreach (NoteNode startingNode in FindStartingNoteNodes(notesInScale[0], scaleFinderOptions))
            {
                await FindAllScalesAsync(results, startingNode, notesInScale, 1, startingNode.String, scaleFinderOptions, cancelToken);
            }

            return results;
        }

        private static IEnumerable<NoteNode> FindStartingNoteNodes(InternalNote targetNote, IScaleFinderOptions scaleFinderOptions)
        {
            for (int str = 0; str < scaleFinderOptions.Instrument.NumStrings; str++)
            {
                FullInternalNote rootNote = scaleFinderOptions.Tuning.RootNotes[str].ToFullInternalNote();

                int fret = NoteUtils.GetShift(rootNote.Note, targetNote);

                while (fret <= scaleFinderOptions.MaxFret)
                {
                    FullInternalNote newNote = rootNote.Shift(fret);
                    yield return new NoteNode(str, fret, newNote, null);
                    fret += 12;
                }
            }
        }

        private static async Task FindAllScalesAsync(ScaleFinderResultSet results, NoteNode noteNode, InternalNote[] targetNotes, int nextNote, int str, IScaleFinderOptions scaleFinderOptions, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return;
            }

            IInstrument instrument = scaleFinderOptions.Instrument;

            if (nextNote == targetNotes.Length) // Build a scale result
            {
                List<MarkPosition> marks = new List<MarkPosition>();

                NoteNode nn = noteNode;

                // Walk back up the tree to set the marks on the result and flag each target note
                while (null != nn)
                {
                    if (nn.Fret >= 0)
                    {
                        marks.Add(new MarkPosition(nn.String + 1, nn.Fret));
                    }

                    nn = nn.Parent;
                }

                results.AddResult(marks);
            }
            else if (str < instrument.NumStrings) // Keep building the tree
            {
                // Look at all the notes on the string
                int startingFret = scaleFinderOptions.AllowOpenStrings ? 0 : 1;

                if (null != noteNode && noteNode.String == str)
                {
                    startingFret = noteNode.Fret + 1;
                }

                FullInternalNote stringRootNote = scaleFinderOptions.Tuning.RootNotes[str].ToFullInternalNote();

                for (int fret = startingFret; fret <= scaleFinderOptions.MaxFret; fret++)
                {
                    FullInternalNote note = stringRootNote.Shift(fret);

                    // See if the note is the next target note
                    bool correctNote = note.Note == targetNotes[nextNote];
                    if (correctNote && scaleFinderOptions.StrictIntervals)
                    {
                        // Check that it's the right number of steps from the previous interval
                        int intervalSteps = scaleFinderOptions.Scale.Intervals[nextNote] - scaleFinderOptions.Scale.Intervals[nextNote - 1];
                        correctNote = correctNote && noteNode.Note.GetShift(note) == intervalSteps;
                    }

                    if (correctNote)
                    {
                        NoteNode child = new NoteNode(str, fret, note, noteNode); // Add found note
                        await FindAllScalesAsync(results, child, targetNotes, nextNote + 1, str, scaleFinderOptions, cancelToken); // Look for the next note on the same string
                    }
                }

                await FindAllScalesAsync(results, noteNode, targetNotes, nextNote, str + 1, scaleFinderOptions, cancelToken); // Look for the next note on the next string
            }
        }

        protected class NoteNode
        {
            public int String { get; set; }
            public int Fret { get; set; }
            public FullInternalNote Note { get; set; }
            public NoteNode Parent { get; set; }

            public NoteNode(int @string, int fret, FullInternalNote note, NoteNode parent)
            {
                String = @string;
                Fret = fret;
                Note = note;
                Parent = parent;
            }

            public NoteNode(int @string) : this(@string, -1, null, null) { }

            public NoteNode(int @string, NoteNode parent) : this(@string, -1, null, parent) { }
        }
    }
}
