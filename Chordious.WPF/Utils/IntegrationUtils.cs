// 
// IntegrationUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2019 Jon Thysell <http://jonthysell.com>
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
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows;

using com.jonthysell.Chordious.Core.ViewModel;

namespace com.jonthysell.Chordious.WPF
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

    }

    public enum DiagramLibraryNodeDragDropAction
    {
        None,
        Copy,
        Move,
    }
}
