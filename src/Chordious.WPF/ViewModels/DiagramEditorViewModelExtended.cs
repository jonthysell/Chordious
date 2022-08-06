// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.ObjectModel;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
{
    public class DiagramEditorViewModelExtended : DiagramEditorViewModel
    {
        public static string SelectedEditorRenderBackgroundLabel
        {
            get
            {
                return Strings.DiagramEditorSelectedEditorRendererBackgroundLabel;
            }
        }

        public static string SelectedEditorRenderBackgroundToolTip
        {
            get
            {
                return Strings.DiagramEditorSelectedEditorRendererBackgroundToolTip;
            }
        }

        public int SelectedEditorRenderBackgroundIndex
        {
            get
            {
                return (int)EditorRenderBackground;
            }
            set
            {
                EditorRenderBackground = (Background)(value);
                OnPropertyChanged(nameof(SelectedEditorRenderBackgroundIndex));
            }
        }

        public static ObservableCollection<string> EditorRenderBackgrounds
        {
            get
            {
                return ImageUtils.GetBackgrounds();
            }
        }

        private Background EditorRenderBackground
        {
            get
            {

                if (Enum.TryParse(AppVM.GetSetting("diagrameditor.renderbackground"), out Background result))
                {
                    return result;
                }

                return Background.None;
            }
            set
            {
                AppVM.SetSetting("diagrameditor.renderbackground", value);
                OnPropertyChanged(nameof(EditorRenderBackground));
                ObservableDiagram.Refresh();
            }
        }

        public DiagramEditorViewModelExtended(ObservableDiagram diagram, bool isNew) : base(diagram, isNew) { }

    }
}
