// 
// ScaleFinder.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015 Jon Thysell <http://jonthysell.com>
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
    public class ScaleFinder
    {
        public static ScaleFinderResultSet FindScales(ScaleFinderOptions scaleFinderOptions)
        {
            scaleFinderOptions = scaleFinderOptions.Clone();

            Note root = scaleFinderOptions.RootNote;
            Scale scale = scaleFinderOptions.Scale;

            InternalNote[] notesInScale = scale.GetNotes(NoteUtils.ToInternalNote(root));

            ScaleFinderResultSet results = new ScaleFinderResultSet(scaleFinderOptions);

            FindAllScales(results, null, notesInScale, 0, 0, scaleFinderOptions);

            return results;
        }

        private static void FindAllScales(ScaleFinderResultSet results, NoteNode noteNode, InternalNote[] targetNotes, int nextNote, int str, ScaleFinderOptions scaleFinderOptions)
        {
            Instrument instrument = scaleFinderOptions.Instrument;

            if (str == instrument.NumStrings || nextNote == targetNotes.Length) // Build a scale result
            {
                List<MarkPosition> marks = new List<MarkPosition>();

                NoteNode nn = noteNode;

                bool[] hasNotes = new bool[targetNotes.Length];

                // Walk back up the tree to set the marks on the result and flag each target note
                int i = targetNotes.Length - 1;
                while (nn != null)
                {
                    if (nn.Fret >= 0)
                    {
                        marks.Insert(0, new MarkPosition(nn.String + 1, nn.Fret));

                        if (nn.Note == targetNotes[i])
                        {
                            hasNotes[i] = true;
                            i--;
                        }
                    }

                    nn = nn.Parent;
                }

                // Add result if it had all the target notes
                bool valid = true;
                for (int j = 0; j < hasNotes.Length; j++)
                {
                    valid = valid && hasNotes[j];
                }

                if (valid)
                {
                    results.AddResult(marks);
                }
            }
            else // Keep building the tree
            {
                // Look at the muted string
                if (scaleFinderOptions.AllowMutedStrings && nextNote == 0)
                {
                    NoteNode muted = new NoteNode(str, noteNode);
                    FindAllScales(results, muted, targetNotes, nextNote, str + 1, scaleFinderOptions);
                }

                // Look at all the notes on the string
                int startingFret = scaleFinderOptions.AllowOpenStrings ? 0 : 1;

                if (null != noteNode && noteNode.String == str)
                {
                    startingFret = noteNode.Fret + 1;
                }

                for (int fret = startingFret; fret <= scaleFinderOptions.MaxFret; fret++)
                {
                    InternalNote note = scaleFinderOptions.Tuning.InternalNoteAt(str, fret);

                    // See if the note is the next target note
                    if (note == targetNotes[nextNote])
                    {
                        NoteNode child = new NoteNode(str, fret, note, noteNode);
                        FindAllScales(results, child, targetNotes, nextNote + 1, str, scaleFinderOptions); // Look for the next note on the same string
                        noteNode = child;
                        break;
                    }
                }

                if (str + 1 < instrument.NumStrings && nextNote + 1 < targetNotes.Length)
                {
                    FindAllScales(results, noteNode, targetNotes, nextNote + 1, str + 1, scaleFinderOptions); // Look for the next note (if there is one) on the next string (if there is one)
                }
            }
        }

        protected class NoteNode
        {
            public int String { get; set; }
            public int Fret { get; set; }
            public InternalNote? Note { get; set; }
            public NoteNode Parent { get; set; }

            public NoteNode(int @string, int fret, InternalNote? note, NoteNode parent)
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
