// 
// Chord.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015, 2017, 2019 Jon Thysell <http://jonthysell.com>
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

namespace com.jonthysell.Chordious.Core.Legacy
{
    internal class Chord
    {
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value ?? throw new ArgumentNullException();
            }
        }
        private string _title;

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    return Title;
                }
                else
                {
                    return _fileName;
                }
            }
            set
            {
                _fileName = value;
            }
        }
        private string _fileName;

        public bool FileNameSet
        {
            get
            {
                return !string.IsNullOrEmpty(_fileName);
            }
        }

        public int NumStrings
        {
            get
            {
                return _numStrings;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _numStrings = value;

                // Init _marks
                if (null == _marks)
                {
                    _marks = new int[_numStrings];
                }
                else if (_marks.Length != _numStrings)
                {
                    int[] temp = new int[_numStrings];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = i < _marks.Length ? _marks[i] : 0;
                    }
                    _marks = temp;
                }
            }
        }
        private int _numStrings;

        public int NumFrets
        {
            get
            {
                return _numFrets;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _numFrets = value;
            }
        }
        private int _numFrets;

        public int BaseLine
        {
            get
            {
                return _baseLine;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _baseLine = value;
            }
        }
        private int _baseLine;

        public int Barre
        {
            get
            {
                return _barre;
            }
            set
            {
                _barre = Math.Max(-1, value);
            }
        }
        private int _barre;

        public int[] Marks
        {
            get
            {
                return _marks;
            }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                // Init _marks
                if (null == _marks)
                {
                    _marks = new int[_numStrings];
                }

                for (int i = 0; i < _marks.Length; i++)
                {
                    _marks[i] = i < value.Length ? value[i] : 0;
                }
            }
        }
        private int[] _marks;

        public Chord(string fileName, string title, int numStrings, int numFrets, int baseLine, int barre, int[] marks)
        {
            Title = title;
            FileName = fileName;
            NumStrings = numStrings;
            NumFrets = numFrets;
            BaseLine = baseLine;
            Barre = barre;
            Marks = marks;
        }

        public Chord(string chordLine)
        {
            if (string.IsNullOrEmpty(chordLine))
            {
                throw new ArgumentNullException("chordLine");
            }

            string[] s = chordLine.Trim().Split(';');

            string[] name_parts = s[0].Split(':');

            string fileName = name_parts.Length > 1 ? name_parts[0] : "";
            string title = name_parts.Length > 1 ? name_parts[1] : name_parts[0];

            Title = title;
            FileName = fileName;

            NumStrings = int.Parse(s[1]);
            NumFrets = int.Parse(s[2]);
            BaseLine = int.Parse(s[3]);
            Barre = int.Parse(s[4]);

            int[] marks = new int[(Math.Min(NumStrings, s.Length - 5))];
            for (int i = 0; i < marks.Length; i++)
            {
                marks[i] = int.Parse(s[5 + i]);
            }
            Marks = marks;
        }

        public void CopyTo(Chord c)
        {
            if (null == c)
            {
                throw new ArgumentNullException("c");
            }

            c.Title = Title;
            c.FileName = FileNameSet ? _fileName : "";
            c.NumStrings = NumStrings;
            c.NumFrets = NumFrets;
            c.BaseLine = BaseLine;
            c.Barre = Barre;
            c.Marks = (int[])Marks.Clone();
        }

        public override string ToString()
        {
            string s = "";

            if (!string.IsNullOrEmpty(_fileName))
            {
                s += _fileName + ":";
            }

            s += Title + ";";

            s += string.Format("{0};{1};{2};{3}", NumStrings, NumFrets, BaseLine, Barre);

            for (int i = 0; i < Marks.Length; i++)
            {
                s+= ";" + Marks[i];
            }

            return s;
        }
    }
}
