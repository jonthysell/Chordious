// 
// EnumUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2016 Jon Thysell <http://jonthysell.com>
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
using System.Collections;
using System.Collections.Generic;

using com.jonthysell.Chordious.Core.Resources;

namespace com.jonthysell.Chordious.Core
{
    public class EnumUtils
    {
        public static string GetFriendlyValue<TEnum>(string value) where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse<TEnum>(value, out result))
            {
                return GetFriendlyValue(result);
            }

            return GetFriendlyValue(value);
        }

        public static string GetFriendlyValue(DiagramOrientation value)
        {
            switch (value)
            {
                case DiagramOrientation.UpDown:
                    return Strings.DiagramOrientationUpDownFriendlyValue;
                case DiagramOrientation.LeftRight:
                    return Strings.DiagramOrientationLeftRightFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        private static string GetFriendlyValue(object value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            return value.ToString();
        }
    }
}
