// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Chordious.Core.ViewModel
{
    public interface IAppView
    {
        void DoOnUIThread(Action action);

        object DoOnUIThread(Func<object> func);

        Stream GetAppConfigStream();

        Stream GetUserConfigStreamToRead();

        Stream GetUserConfigStreamToWrite();

        object SvgTextToImage(string svgText, int width, int height, bool editMode);

        void TextToClipboard(string text);

        void DiagramToClipboard(ObservableDiagram diagram, float scaleFactor);

        IEnumerable<string> GetSystemFonts();
    }
}
