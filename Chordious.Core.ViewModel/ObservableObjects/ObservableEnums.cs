// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

namespace Chordious.Core.ViewModel
{
    public class ObservableEnums
    {
        public static ObservableCollection<string> GetNotes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            for (int i = 0; i < Enum.GetValues(typeof(Note)).Length; i++)
            {
                collection.Add(NoteUtils.ToString((Note)i));
            }

            return collection;
        }

        public static ObservableCollection<string> GetInternalNotes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            for (int i = 0; i < Enum.GetValues(typeof(InternalNote)).Length; i++)
            {
                collection.Add(NoteUtils.ToString((InternalNote)i, InternalNoteStringStyle.ShowBoth));
            }

            return collection;
        }

        public static ObservableCollection<string> FontFamilies
        {
            get
            {
                return _fontFamilies ?? (_fontFamilies = GetFontFamilies());
            }
        }
        private static ObservableCollection<string> _fontFamilies;

        public static ObservableCollection<string> GetDiagramLabelStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramLabelStyle.Regular),
                EnumUtils.GetFriendlyValue(DiagramLabelStyle.ChordName)
            };

            return collection;
        }

        public static ObservableCollection<string> GetOrientations()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramOrientation.UpDown),
                EnumUtils.GetFriendlyValue(DiagramOrientation.LeftRight)
            };

            return collection;
        }

        public static ObservableCollection<string> GetLabelLayoutModels()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.Overlap),
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingHorizontal),
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingVertical),
                EnumUtils.GetFriendlyValue(DiagramLabelLayoutModel.AddPaddingBoth)
            };

            return collection;
        }

        public static ObservableCollection<string> GetHorizontalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Left),
                EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Center),
                EnumUtils.GetFriendlyValue(DiagramHorizontalAlignment.Right)
            };

            return collection;
        }

        public static ObservableCollection<string> GetVerticalAlignments()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Top),
                EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Middle),
                EnumUtils.GetFriendlyValue(DiagramVerticalAlignment.Bottom)
            };

            return collection;
        }

        public static ObservableCollection<string> GetTextStyles()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramTextStyle.Regular),
                EnumUtils.GetFriendlyValue(DiagramTextStyle.Bold),
                EnumUtils.GetFriendlyValue(DiagramTextStyle.Italic),
                EnumUtils.GetFriendlyValue(DiagramTextStyle.BoldItalic)
            };

            return collection;
        }

        private static ObservableCollection<string> GetFontFamilies()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>(AppViewModel.Instance.AppView.GetSystemFonts());

            collection.SortedInsert("serif");
            collection.SortedInsert("sans-serif");
            collection.SortedInsert("monospace");

            return collection;
        }


        public static ObservableCollection<string> GetMarkTypes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramMarkType.Normal),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Muted),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Root),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Open),
                EnumUtils.GetFriendlyValue(DiagramMarkType.OpenRoot),
                EnumUtils.GetFriendlyValue(DiagramMarkType.Bottom)
            };

            return collection;
        }

        public static ObservableCollection<string> GetMarkShapes()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramMarkShape.None),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.Circle),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.Square),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.Diamond),
                EnumUtils.GetFriendlyValue(DiagramMarkShape.X)
            };

            return collection;
        }

        public static ObservableCollection<string> GetBarreStacks()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(DiagramBarreStack.UnderMarks),
                EnumUtils.GetFriendlyValue(DiagramBarreStack.OverMarks)
            };

            return collection;
        }

        public static ObservableCollection<string> GetBarreTypeOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(BarreTypeOption.None),
                EnumUtils.GetFriendlyValue(BarreTypeOption.Partial),
                EnumUtils.GetFriendlyValue(BarreTypeOption.Full)
            };

            return collection;
        }

        public static ObservableCollection<string> GetMarkTextOptions()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(MarkTextOption.None),
                EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_ShowBoth),
                EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_PreferFlats),
                EnumUtils.GetFriendlyValue(MarkTextOption.ShowNote_PreferSharps)
            };

            return collection;
        }

        public static ObservableCollection<string> GetFretLabelSides()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>
            {
                EnumUtils.GetFriendlyValue(FretLabelSide.Left),
                EnumUtils.GetFriendlyValue(FretLabelSide.Right)
            };

            return collection;
        }
    }
}
