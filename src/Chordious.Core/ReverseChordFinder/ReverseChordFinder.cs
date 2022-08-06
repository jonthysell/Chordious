// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

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
            if (reverseChordFinderOptions is null)
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
