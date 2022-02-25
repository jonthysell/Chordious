// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Chordious.Core.ViewModel;

namespace Chordious.WPF
{
    public class IntegrationUtils
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public static string TempPath
        {
            get
            {
                return _tempPath ?? (_tempPath = Path.Combine(Path.GetTempPath(), "Chordious"));
            }
        }
        private static string _tempPath;

        public static double DpiScale
        {
            get
            {
                return (double)AppVM.AppView.DoOnUIThread(() =>
                {
                    return VisualTreeHelper.GetDpi(Application.Current.MainWindow).DpiScaleY;
                });
            }
        }

        public static string GetTempPathAndCreateIfNecessary()
        {
            string tempPath = TempPath;

            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            return tempPath;
        }

        public static void TextToClipboard(string text)
        {
            Clipboard.SetText(text);
            Clipboard.Flush();
        }

        public static void DiagramToClipboard(ObservableDiagram diagram, float scaleFactor)
        {
            if (null == diagram)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            DataObject data = new DataObject();

            AddImageFormats(data, diagram, scaleFactor);

            Clipboard.SetDataObject(data, true);

            Clipboard.Flush();
        }

        public static void DiagramToDragDrop(DependencyObject dragSource, ObservableDiagram diagram)
        {
            if (null == dragSource)
            {
                throw new ArgumentNullException(nameof(dragSource));
            }

            if (null == diagram)
            {
                throw new ArgumentNullException(nameof(diagram));
            }

            DataObject data = new DataObject();

            AddImageFormats(data, diagram, 1.0f);

            DragDrop.DoDragDrop(dragSource, data, DragDropEffects.Copy | DragDropEffects.Move);
        }

        public static void DiagramLibraryNodeToDragDrop(DependencyObject dragSource, ObservableDiagramLibraryNode diagramLibraryNode, bool useSelectedDiagrams)
        {
            if (null == dragSource)
            {
                throw new ArgumentNullException(nameof(dragSource));
            }

            if (null == diagramLibraryNode)
            {
                throw new ArgumentNullException(nameof(diagramLibraryNode));
            }

            DataObject data = new DataObject();

            data.SetData(typeof(ObservableDiagramLibraryNode), diagramLibraryNode);
            data.SetData(UseSelectedDiagramsFormat, useSelectedDiagrams);

            if (useSelectedDiagrams && diagramLibraryNode.SelectedDiagrams.Count > 0)
            {
                AddImageFormats(data, diagramLibraryNode.SelectedDiagrams[0], 1.0f);
            }

            DragDrop.DoDragDrop(dragSource, data, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private static void AddImageFormats(DataObject data, ObservableDiagram diagram, float scaleFactor)
        {
            Bitmap bmp = ImageUtils.SvgTextToBitmap(diagram.SvgText, diagram.TotalWidth, diagram.TotalHeight, scaleFactor);

            // Standard bitmap, no transparency
            data.SetData(DataFormats.Bitmap, ImageUtils.AddBackground(bmp, Background.White));

            // As PNG
            data.SetData("PNG", ImageUtils.BitmapToPngStream(bmp));

            // As EMF
            data.SetData(DataFormats.EnhancedMetafile, ImageUtils.BitmapToMetafileStream(bmp));

            // As PNG temp file
            if (GetEnhancedCopy())
            {
                data.SetFileDropList(new StringCollection { ImageUtils.BitmapToPngTempFile(bmp, diagram.Title) });
            }
        }

        public static void DragDropToDiagramLibraryNode(IDataObject data, ObservableDiagramLibraryNode destinationNode, DiagramLibraryNodeDragDropAction dragDropAction)
        {
            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.GetDataPresent(typeof(ObservableDiagramLibraryNode)) && data.GetData(typeof(ObservableDiagramLibraryNode)) is ObservableDiagramLibraryNode sourceNode)
            {
                bool useSelectedDiagrams = data.GetData(UseSelectedDiagramsFormat) as bool? ?? false;

                if (sourceNode == destinationNode)
                {
                    if (dragDropAction == DiagramLibraryNodeDragDropAction.Copy)
                    {
                        if (useSelectedDiagrams)
                        {
                            // Cloning diagrams within the same collection
                            if (useSelectedDiagrams && sourceNode.CloneSelected.CanExecute(null))
                            {
                                sourceNode.CloneSelected.Execute(null);
                            }
                        }
                    }
                }
                else
                {
                    if (dragDropAction == DiagramLibraryNodeDragDropAction.Copy)
                    {
                        if (useSelectedDiagrams)
                        {
                            // Copy selected diagrams into the destination
                            if (sourceNode.CopySelected.CanExecute(destinationNode?.Name))
                            {
                                sourceNode.CopySelected.Execute(destinationNode?.Name);
                            }
                        }
                        else
                        {
                            // Copy all diagrams into the destination
                            if (sourceNode.CopyNode.CanExecute(null))
                            {
                                sourceNode.CopyNode.Execute(destinationNode?.Name);
                            }
                        }
                    }
                    else if (dragDropAction == DiagramLibraryNodeDragDropAction.Move)
                    {
                        if (useSelectedDiagrams)
                        {
                            // Move all selected diagrams into the destination
                            if (sourceNode.MoveSelected.CanExecute(destinationNode?.Name))
                            {
                                sourceNode.MoveSelected.Execute(destinationNode?.Name);
                            }
                        }
                        else
                        {
                            // Move all diagrams into the destination (merge)
                            if (sourceNode.MergeNode.CanExecute(null))
                            {
                                sourceNode.MergeNode.Execute(destinationNode?.Name);
                            }
                        }
                    }
                }
            }
        }

        public static DiagramLibraryNodeDragDropAction GetDropAction(DragDropKeyStates states)
        {
            if (states.HasFlag(DragDropKeyStates.ControlKey))
            {
                return DiagramLibraryNodeDragDropAction.Copy;
            }

            return DiagramLibraryNodeDragDropAction.Move;
        }

        public static void EnableAutoSelectOnFirstLoad(TextBox textBox)
        {
            textBox.TextChanged += AutoSelectOnFirstLoad_TextChanged;
        }

        private static void AutoSelectOnFirstLoad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!textBox.IsReadOnly)
                {
                    textBox.SelectAll();
                }
                textBox.TextChanged -= AutoSelectOnFirstLoad_TextChanged;
            }
        }

        #region Settings

        public static bool GetEnhancedCopy()
        {
            if (bool.TryParse(AppVM.GetSetting("integration.enhancedcopy"), out bool result))
            {
                return result;
            }

            return false;
        }

        #endregion

        public const string UseSelectedDiagramsFormat = "UseSelectedDiagrams";

        #region Licenses

        public static ObservableLicense GetSvgNetLicense()
        {
            return new ObservableLicense("SVG.NET", "Copyright © 2013-2022 vvvv.org", "Microsoft Public License (Ms-PL)", string.Join(Environment.NewLine + Environment.NewLine, _msPlLicense));
        }

        public static ObservableLicense GetExtendedWPFToolkitLicense()
        {
            return new ObservableLicense("Wpf Extended Toolkit", "Copyright © 2007-2018 Xceed Software Inc.", "Microsoft Public License (Ms-PL)", string.Join(Environment.NewLine + Environment.NewLine, _msPlLicense));
        }

        private static readonly string[] _msPlLicense = {
            @"This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.",
            @"1. Definitions",
            @"The terms ""reproduce,"" ""reproduction,"" ""derivative works,"" and ""distribution"" have the same meaning here as under U.S. copyright law.",
            @"A ""contribution"" is the original software, or any additions or changes to the software.",
            @"A ""contributor"" is any person that distributes its contribution under this license.",
            @"""Licensed patents"" are a contributor's patent claims that read directly on its contribution.",
            @"2. Grant of Rights",
            @"(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.",
            @"(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.",
            @"3. Conditions and Limitations",
            @"(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.",
            @"(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.",
            @"(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.",
            @"(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.",
            @"(E) The software is licensed ""as-is."" You bear the risk of using it. The contributors give no express warranties, guarantees or conditions.You may have additional consumer rights under your local laws which this license cannot change.To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.",
        };

        #endregion
    }

    public enum DiagramLibraryNodeDragDropAction
    {
        None,
        Copy,
        Move,
    }
}
