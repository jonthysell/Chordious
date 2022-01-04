// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Chordious.Core
{
    public interface IFinderOptions2 : IFinderOptions
    {
        Note RootNote { get; }

        int NumFrets { get; }
        int MaxFret { get; }
        int MaxReach { get; }
        bool AllowOpenStrings { get; }
        bool AllowMutedStrings { get; }
    }
}
