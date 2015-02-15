// 
// ChordFinder.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core
{
    public class ChordFinder
    {
        public static ChordFinderResultSet FindChords(ChordFinderOptions chordFinderOptions)
        {
            chordFinderOptions = chordFinderOptions.Clone();

            Note root = chordFinderOptions.RootNote;
            ChordQuality chordQuality = chordFinderOptions.ChordQuality;

            InternalNote[] notesInChord = chordQuality.GetNotes(NoteUtils.ToInternalNote(root));

            ChordFinderResultSet results = new ChordFinderResultSet(chordFinderOptions);

            FindAllChords(results, null, notesInChord, 0, chordFinderOptions);

            return results;
        }

        private static void FindAllChords(ChordFinderResultSet results, NoteNode noteNode, InternalNote[] targetNotes, int str, ChordFinderOptions chordFinderOptions)
        {
            Instrument instrument = chordFinderOptions.Instrument;

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
                            break;
                        }
                    }

                    nn = nn.Parent;
                    str--;
                }

                // The first target note is always the root, so ignore if we want
                // to allow rootless chords
                int firstTargetNote = chordFinderOptions.AllowRootlessChords ? 1 : 0;

                // Add result if it had all the target notes
                bool valid = true;
                for (int i = firstTargetNote; i < hasNotes.Length; i++)
                {
                    valid = valid && hasNotes[i];
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
                    FindAllChords(results, muted, targetNotes, str + 1, chordFinderOptions);
                }

                // Look at all the notes on the string
                int startingFret = chordFinderOptions.AllowOpenStrings ? 0 : 1;
                for (int fret = startingFret; fret <= chordFinderOptions.MaxFret; fret++)
                {
                    InternalNote note = chordFinderOptions.Tuning.NoteAt(str, fret);

                    // See if the note is a target note
                    for (int i = 0; i < targetNotes.Length; i++)
                    {
                        // If it's a target note add it and search on the next string
                        if (note == targetNotes[i])
                        {
                            NoteNode child = new NoteNode(fret, note, noteNode);
                            FindAllChords(results, child, targetNotes, str + 1, chordFinderOptions);
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