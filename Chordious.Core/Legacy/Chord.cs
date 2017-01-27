// 
// Chord.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2013, 2014, 2015, 2017 Jon Thysell <http://jonthysell.com>
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
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                _title = value;
            }
        }
        private string _title;

        public string FileName
        {
            get
            {
                if (String.IsNullOrEmpty(this._fileName))
                {
                    return this.Title;
                }
                else
                {
                    return this._fileName;
                }
            }
            set
            {
                this._fileName = value;
            }
        }
        private string _fileName;

        public bool FileNameSet
        {
            get
            {
                return !String.IsNullOrEmpty(this._fileName);
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
            this.Title = title;
            this.FileName = fileName;
            this.NumStrings = numStrings;
            this.NumFrets = numFrets;
            this.BaseLine = baseLine;
            this.Barre = barre;
            this.Marks = marks;
        }

        public Chord(string chordLine)
        {
            if (String.IsNullOrEmpty(chordLine))
            {
                throw new ArgumentNullException("chordLine");
            }

            string[] s = chordLine.Trim().Split(';');

            string[] name_parts = s[0].Split(':');

            string fileName = name_parts.Length > 1 ? name_parts[0] : "";
            string title = name_parts.Length > 1 ? name_parts[1] : name_parts[0];

            this.Title = title;
            this.FileName = fileName;

            this.NumStrings = Int32.Parse(s[1]);
            this.NumFrets = Int32.Parse(s[2]);
            this.BaseLine = Int32.Parse(s[3]);
            this.Barre = Int32.Parse(s[4]);

            int[] marks = new int[(int)Math.Min(this.NumStrings, s.Length - 5)];
            for (int i = 0; i < marks.Length; i++)
            {
                marks[i] = Int32.Parse(s[5 + i]);
            }
            this.Marks = marks;
        }

        public void CopyTo(Chord c)
        {
            if (null == c)
            {
                throw new ArgumentNullException("c");
            }

            c.Title = this.Title;
            c.FileName = this.FileNameSet ? this._fileName : "";
            c.NumStrings = this.NumStrings;
            c.NumFrets = this.NumFrets;
            c.BaseLine = this.BaseLine;
            c.Barre = this.Barre;
            c.Marks = (int[])this.Marks.Clone();
        }

        public override string ToString()
        {
            string s = "";

            if (!String.IsNullOrEmpty(_fileName))
            {
                s += _fileName + ":";
            }

            s += Title + ";";

            s += String.Format("{0};{1};{2};{3}", NumStrings, NumFrets, BaseLine, Barre);

            for (int i = 0; i < this.Marks.Length; i++)
            {
                s+= ";" + this.Marks[i];
            }

            return s;
        }
    }
}
