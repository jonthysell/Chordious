// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public static class NoteUtils
    {
        public static IEnumerable<InternalNote> InternalNotes
        {
            get
            {
                return (InternalNote[])Enum.GetValues(typeof(InternalNote));
            }
        }

        public static Note ParseNote(string s)
        {
            if (TryParseNote(s, out Note note))
            {
                return note;
            }

            throw new ArgumentException(Strings.InvalidNoteArgumentExceptionMessage);
        }

        public static bool TryParseNote(string s, out Note note)
        {
            if (!StringUtils.IsNullOrWhiteSpace(s))
            {
                try
                {
                    note = (Note)Enum.Parse(typeof(Note), s.Replace('#', 's'));
                    return true;
                }
                catch (Exception) { }
            }

            note = default;
            return false;
        }

        public static string ToString(Note note)
        {
            return note.ToString().Replace("s", "#");
        }

        public static string ToString(InternalNote note, InternalNoteStringStyle style)
        {
            if (IsNatural(note) || style != InternalNoteStringStyle.ShowBoth)
            {
                return ToString(ToNote(note, style));
            }
            else
            {
                switch (note)
                {
                    case InternalNote.Cs_Db:
                        return "C#/Db";
                    case InternalNote.Ds_Eb:
                        return "D#/Eb";
                    case InternalNote.Fs_Gb:
                        return "F#/Gb";
                    case InternalNote.Gs_Ab:
                        return "G#/Ab";
                    case InternalNote.As_Bb:
                        return "A#/Bb";
                }
            }

            throw new ArgumentException();
        }

        public static InternalNote ToInternalNote(Note note)
        {
            switch (note)
            {
                case Note.C:
                    return InternalNote.C;
                case Note.Cs:
                case Note.Db:
                    return InternalNote.Cs_Db;
                case Note.D:
                    return InternalNote.D;
                case Note.Ds:
                case Note.Eb:
                    return InternalNote.Ds_Eb;
                case Note.E:
                    return InternalNote.E;
                case Note.F:
                    return InternalNote.F;
                case Note.Fs:
                case Note.Gb:
                    return InternalNote.Fs_Gb;
                case Note.G:
                    return InternalNote.G;
                case Note.Gs:
                case Note.Ab:
                    return InternalNote.Gs_Ab;
                case Note.A:
                    return InternalNote.A;
                case Note.As:
                case Note.Bb:
                    return InternalNote.As_Bb;
                case Note.B:
                    return InternalNote.B;
            }

            throw new ArgumentException();
        }

        public static Note ToNote(InternalNote note, InternalNoteStringStyle style)
        {
            switch (note)
            {
                case InternalNote.C:
                    return Note.C;
                case InternalNote.Cs_Db:
                    if (style == InternalNoteStringStyle.PreferSharp)
                    {
                        return Note.Cs;
                    }
                    else if (style == InternalNoteStringStyle.PreferFlat)
                    {
                        return Note.Db;
                    }
                    break;
                case InternalNote.D:
                    return Note.D;
                case InternalNote.Ds_Eb:
                    if (style == InternalNoteStringStyle.PreferSharp)
                    {
                        return Note.Ds;
                    }
                    else if (style == InternalNoteStringStyle.PreferFlat)
                    {
                        return Note.Eb;
                    }
                    break;
                case InternalNote.E:
                    return Note.E;
                case InternalNote.F:
                    return Note.F;
                case InternalNote.Fs_Gb:
                    if (style == InternalNoteStringStyle.PreferSharp)
                    {
                        return Note.Fs;
                    }
                    else if (style == InternalNoteStringStyle.PreferFlat)
                    {
                        return Note.Gb;
                    }
                    break;
                case InternalNote.G:
                    return Note.G;
                case InternalNote.Gs_Ab:
                    if (style == InternalNoteStringStyle.PreferSharp)
                    {
                        return Note.Gs;
                    }
                    else if (style == InternalNoteStringStyle.PreferFlat)
                    {
                        return Note.Ab;
                    }
                    break;
                case InternalNote.A:
                    return Note.A;
                case InternalNote.As_Bb:
                    if (style == InternalNoteStringStyle.PreferSharp)
                    {
                        return Note.As;
                    }
                    else if (style == InternalNoteStringStyle.PreferFlat)
                    {
                        return Note.Bb;
                    }
                    break;
                case InternalNote.B:
                    return Note.B;
            }

            throw new ArgumentException();
        }

        public static InternalNote Shift(InternalNote root, int steps)
        {
            if (steps == 0)
            {
                return root;
            }

            int newNote = ((int)root + steps);

            while (newNote < 0)
            {
                newNote += 12;
            }

            return (InternalNote)(newNote % 12);
        }

        public static int GetShift(InternalNote root, InternalNote targetNote)
        {
            int shift = (int)targetNote - (int)root;

            while (shift < 0)
            {
                shift += 12;
            }

            return shift;
        }

        public static bool IsNatural(Note note)
        {
            return Array.IndexOf<Note>(NaturalNotes, note) >= 0;
        }

        private static readonly Note[] NaturalNotes = { Note.C, Note.D, Note.E, Note.F, Note.G, Note.A, Note.B };

        public static bool IsNatural(InternalNote note)
        {
            return Array.IndexOf<InternalNote>(NaturalInternalNotes, note) >= 0;
        }

        private static readonly InternalNote[] NaturalInternalNotes = { InternalNote.C, InternalNote.D, InternalNote.E, InternalNote.F, InternalNote.G, InternalNote.A, InternalNote.B };
    }

    public enum Note
    {
        C = 0,
        Cs,
        Db,
        D,
        Ds,
        Eb,
        E,
        F,
        Fs,
        Gb,
        G,
        Gs,
        Ab,
        A,
        As,
        Bb,
        B
    }

    public enum InternalNote
    {
        C = 0,
        Cs_Db,
        D,
        Ds_Eb,
        E,
        F,
        Fs_Gb,
        G,
        Gs_Ab,
        A,
        As_Bb,
        B
    }

    public enum InternalNoteStringStyle
    {
        ShowBoth,
        PreferSharp,
        PreferFlat
    }
}
