﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;

namespace Chordious.Core
{
    public delegate void MarkTypeChangedEventHandler(object sender, EventArgs e);

    public class DiagramMarkStyleWrapper
    {
        public DiagramMarkType MarkType
        {
            get
            {
                return _markType;
            }
            set
            {
                _markType = value;
                OnMarkTypeChanged();
            }
        }
        private DiagramMarkType _markType;

        public bool MarkShapeIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.shape", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.shape", value, MarkType);
            }
        }

        public DiagramMarkShape MarkShape
        {
            get
            {
                return Style.MarkStyleGetEnum<DiagramMarkShape>("mark.shape", MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.shape", value, MarkType);
            }
        }

        public bool MarkVisibleIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.visible", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.visible", value, MarkType);
            }
        }

        public bool MarkVisible
        {
            get
            {
                return Style.MarkStyleGetBoolean("mark.visible", MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.visible", value, MarkType);
            }
        }

        public bool MarkColorIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.color", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.color", value, MarkType);
            }
        }

        public string MarkColor
        {
            get
            {
                return Style.MarkStyleGetColor("mark.color", MarkType);
            }
            set
            {
                Style.MarkStyleSetColor("mark.color", value, MarkType);
            }
        }

        public bool MarkOpacityIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.opacity", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.opacity", value, MarkType);
            }
        }

        public double MarkOpacity
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.opacity", MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Style.MarkStyleSet("mark.opacity", value, MarkType);
            }
        }

        public bool MarkTextStyleIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.textstyle", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.textstyle", value, MarkType);
            }
        }

        public DiagramTextStyle MarkTextStyle
        {
            get
            {
                return Style.MarkStyleGetEnum<DiagramTextStyle>("mark.textstyle", MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.textstyle", value, MarkType);
            }
        }

        public bool MarkTextAlignmentIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.textalignment", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.textalignment", value, MarkType);
            }
        }

        public DiagramHorizontalAlignment MarkTextAlignment
        {
            get
            {
                return Style.MarkStyleGetEnum<DiagramHorizontalAlignment>("mark.textalignment", MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.textalignment", value, MarkType);
            }
        }

        public bool MarkTextSizeRatioIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.textsizeratio", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.textsizeratio", value, MarkType);
            }
        }

        public double MarkTextSizeRatio
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.textsizeratio", MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Style.MarkStyleSet("mark.textsizeratio", value, MarkType);
            }
        }

        public bool MarkTextVisibleIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.textvisible", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.textvisible", value, MarkType);
            }
        }

        public bool MarkTextVisible
        {
            get
            {
                return Style.MarkStyleGetBoolean("mark.textvisible", MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.textvisible", value, MarkType);
            }
        }

        public bool MarkTextColorIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.textcolor", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.textcolor", value, MarkType);
            }
        }

        public string MarkTextColor
        {
            get
            {
                return Style.MarkStyleGetColor("mark.textcolor", MarkType);
            }
            set
            {
                Style.MarkStyleSetColor("mark.textcolor", value, MarkType);
            }
        }

        public bool MarkTextOpacityIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.textopacity", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.textopacity", value, MarkType);
            }
        }

        public double MarkTextOpacity
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.textopacity", MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Style.MarkStyleSet("mark.textopacity", value, MarkType);
            }
        }

        public bool MarkFontFamilyIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.fontfamily", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.fontfamily", value, MarkType);
            }
        }

        public string MarkFontFamily
        {
            get
            {
                return Style.MarkStyleGet("mark.fontfamily", MarkType);
            }
            set
            {
                Style.MarkStyleSet("mark.fontfamily", value, MarkType);
            }
        }

        public bool MarkRadiusRatioIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.radiusratio", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.radiusratio", value, MarkType);
            }
        }

        public double MarkRadiusRatio
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.radiusratio", MarkType);
            }
            set
            {
                if (value < 0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Style.MarkStyleSet("mark.radiusratio", value, MarkType);
            }
        }

        public bool MarkBorderColorIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.bordercolor", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.bordercolor", value, MarkType);
            }
        }

        public string MarkBorderColor
        {
            get
            {
                return Style.MarkStyleGetColor("mark.bordercolor", MarkType);
            }
            set
            {
                Style.MarkStyleSetColor("mark.bordercolor", value, MarkType);
            }
        }

        public bool MarkBorderThicknessIsLocal
        {
            get
            {
                return Style.MarkStyleIsLocalGet("mark.borderthickness", MarkType);
            }
            set
            {
                Style.MarkStyleIsLocalSet("mark.borderthickness", value, MarkType);
            }
        }

        public double MarkBorderThickness
        {
            get
            {
                return Style.MarkStyleGetDouble("mark.borderthickness", MarkType);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                Style.MarkStyleSet("mark.borderthickness", value, MarkType);
            }
        }

        public DiagramStyle Style { get; private set; }

        public event MarkTypeChangedEventHandler MarkTypeChanged;

        public DiagramMarkStyleWrapper(DiagramStyle style, DiagramMarkType markType = DiagramMarkType.Normal)
        {
            Style = style ?? throw new ArgumentNullException(nameof(style));
            MarkType = markType;
        }

        public void ForEachMarkType(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            DiagramMarkType original = MarkType;

            foreach (DiagramMarkType markType in (DiagramMarkType[])Enum.GetValues(typeof(DiagramMarkType)))
            {
                MarkType = markType;
                action();
            }

            MarkType = original;
        }

        private void OnMarkTypeChanged()
        {
            MarkTypeChanged?.Invoke(this, new EventArgs());
        }
    }
}
