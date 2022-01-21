// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

using Chordious.Core.Resources;

namespace Chordious.Core
{
    public class EnumUtils
    {
        public static string GetFriendlyValue<TEnum>(string value) where TEnum : struct
        {
            if (Enum.TryParse(value, out TEnum result))
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

        public static string GetFriendlyValue(DiagramLabelLayoutModel value)
        {
            switch (value)
            {
                case DiagramLabelLayoutModel.Overlap:
                    return Strings.DiagramLabelLayoutModelOverlapFriendlyValue;
                case DiagramLabelLayoutModel.AddPaddingHorizontal:
                    return Strings.DiagramLabelLayoutModelAddPaddingHorizontalFriendlyValue;
                case DiagramLabelLayoutModel.AddPaddingVertical:
                    return Strings.DiagramLabelLayoutModelAddPaddingVerticalFriendlyValue;
                case DiagramLabelLayoutModel.AddPaddingBoth:
                    return Strings.DiagramLabelLayoutModelAddPaddingBothFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramMarkShape value)
        {
            switch (value)
            {
                case DiagramMarkShape.None:
                    return Strings.DiagramMarkShapeNoneFriendlyValue;
                case DiagramMarkShape.Circle:
                    return Strings.DiagramMarkShapeCircleFriendlyValue;
                case DiagramMarkShape.Square:
                    return Strings.DiagramMarkShapeSquareFriendlyValue;
                case DiagramMarkShape.Diamond:
                    return Strings.DiagramMarkShapeDiamondFriendlyValue;
                case DiagramMarkShape.X:
                    return Strings.DiagramMarkShapeXFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramTextStyle value)
        {
            switch (value)
            {
                case DiagramTextStyle.Regular:
                    return Strings.DiagramTextStyleRegularFriendlyValue;
                case DiagramTextStyle.Bold:
                    return Strings.DiagramTextStyleBoldFriendlyValue;
                case DiagramTextStyle.Italic:
                    return Strings.DiagramTextStyleItalicFriendlyValue;
                case DiagramTextStyle.BoldItalic:
                    return Strings.DiagramTextStyleBoldItalicFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramHorizontalAlignment value)
        {
            switch (value)
            {
                case DiagramHorizontalAlignment.Left:
                    return Strings.DiagramHorizontalAlignmentLeftFriendlyValue;
                case DiagramHorizontalAlignment.Center:
                    return Strings.DiagramHorizontalAlignmentCenterFriendlyValue;
                case DiagramHorizontalAlignment.Right:
                    return Strings.DiagramHorizontalAlignmentRightFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramVerticalAlignment value)
        {
            switch (value)
            {
                case DiagramVerticalAlignment.Top:
                    return Strings.DiagramVerticalAlignmentTopFriendlyValue;
                case DiagramVerticalAlignment.Middle:
                    return Strings.DiagramVerticalAlignmentMiddleFriendlyValue;
                case DiagramVerticalAlignment.Bottom:
                    return Strings.DiagramVerticalAlignmentBottomFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramLabelStyle value)
        {
            switch (value)
            {
                case DiagramLabelStyle.Regular:
                    return Strings.DiagramLabelStyleRegularFriendlyValue;
                case DiagramLabelStyle.ChordName:
                    return Strings.DiagramLabelStyleChordNameFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramBarreStack value)
        {
            switch (value)
            {
                case DiagramBarreStack.UnderMarks:
                    return Strings.DiagramBarreStackUnderMarksFriendlyValue;
                case DiagramBarreStack.OverMarks:
                    return Strings.DiagramBarreStackOverMarksFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(DiagramMarkType value)
        {
            switch (value)
            {
                case DiagramMarkType.Normal:
                    return Strings.DiagramMarkTypeNormalFriendlyValue;
                case DiagramMarkType.Muted:
                    return Strings.DiagramMarkTypeMutedFriendlyValue;
                case DiagramMarkType.Root:
                    return Strings.DiagramMarkTypeRootFriendlyValue;
                case DiagramMarkType.Open:
                    return Strings.DiagramMarkTypeOpenFriendlyValue;
                case DiagramMarkType.OpenRoot:
                    return Strings.DiagramMarkTypeOpenRootFriendlyValue;
                case DiagramMarkType.Bottom:
                    return Strings.DiagramMarkTypeBottomFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(MarkTextOption value)
        {
            switch (value)
            {
                case MarkTextOption.None:
                    return Strings.MarkTextOptionNoneFriendlyValue;
                case MarkTextOption.ShowNote_ShowBoth:
                    return Strings.MarkTextOptionShowNoteShowBothFriendlyValue;
                case MarkTextOption.ShowNote_PreferFlats:
                    return Strings.MarkTextOptionShowNotePreferFlatsFriendlyValue;
                case MarkTextOption.ShowNote_PreferSharps:
                    return Strings.MarkTextOptionShowNotePreferSharpsFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(BarreTypeOption value)
        {
            switch (value)
            {
                case BarreTypeOption.None:
                    return Strings.BarreTypeOptionNoneFriendlyValue;
                case BarreTypeOption.Partial:
                    return Strings.BarreTypeOptionPartialFriendlyValue;
                case BarreTypeOption.Full:
                    return Strings.BarreTypeOptionFullFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        public static string GetFriendlyValue(FretLabelSide value)
        {
            switch (value)
            {
                case FretLabelSide.Left:
                    return Strings.FretLabelSideLeftFriendlyValue;
                case FretLabelSide.Right:
                    return Strings.FretLabelSideRightFriendlyValue;
            }

            return GetFriendlyValue((object)value);
        }

        private static string GetFriendlyValue(object value)
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.ToString();
        }
    }
}
