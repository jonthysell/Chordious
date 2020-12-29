# Chordious Changelist #

## 2.1.X.X ##
* Chord Finder can now find "partial" chords
* Diagram export can now be interrupted, and dialog can optionally close afterward
* Added Extended WPF Toolkit v3.8.2 dependency
* Fixed issue with non-deterministic style summary text
* Extended clickable area for editing fret labels
* Fixed layout inconsistencies with visibility of elements with text

## 2.0.13 ##
* Fixed issue with importing configs with collection styles
* Fixed issue with cloning collections not cloning collection styles
* Fixed issue with resetting styles not updating the UI

## 2.0.12 ##
* Fixed issue with exporting diagrams

## 2.0.11 ##
* Removed unnecessary DLLs
* Fixed issue with text scaling on high DPI monitors
* Fixed issue with licenses UI
* Fixed issue with update check failures

## 2.0.10 ##
* Fixed crash when trying to export diagrams

## 2.0.9 ##
* New WinStore build for the Microsoft Store
* Updated to .NET 4.6.2
* Updated SVG.NET to v2.4.3
* Updated MVVM Light to v5.4.1.1
* Consolidated licenses for dependencies into the UI
* Fixed a bug with the shortcut icon not loading on some machines
* Fixed a bug with the temporary files created during "Enhanced Copy And Drag"
* Fixed a bug where 0% barre arc ratios made the barre disappear
* Code cleanup and miscellaneous bug fixes

## 2.0.8 ##
* Can now specify which side to put fret labels in Finder results
* Prompt to save instrument/tuning when exiting Finders
* Can now run multiple instances as long as they're using different config files
* Fixed bug with double-clicking to edit diagram
* Fixed bug with adding bottom marks not using bottom mark style in editor
* Code cleanup and miscellaneous bug fixes

## 2.0.7 ##
* Improved compatibility when copying a diagram to the clipboard
* Added drag/drop functionality for managing diagrams in the library
* Added drag/drop of diagrams directly into external apps
* Added "Enhanced Copy And Drag" option to improve copy/drag compatibility with external apps

## 2.0.6 ##
* Added Major and Dominant 7th suspended chords
* Fixed Added Tone chords
* Fixed issue with X-shape not appearing unless the mark type was muted
* Updating no longer downloads and runs the MSI, instead takes user to download page

## 2.0.5 ##
* The self-contained ChordiousPortable.exe is now available
* Can now easily copy tunings in the Instrument Manager
* Added context menus to the Finder results
* Cleaned up unnecessary files

## 2.0.4 ##
* Can now view default instruments/tunings
* Can now view default scales/qualities
* Copy to clipboard now standardized across Finders and Library
* Fixed bug where checking for updates gets old cached version

## 2.0.3 ##
* UI is now friendly to Narrator and other screen readers
* Improved keyboard navigation with alt-keys for most controls, 'Enter' to open list items
* Can now copy diagram images directly to the clipboard via context-menu and/or 'Ctrl+C', 'Ctrl+Shift+C'
* Can now exit out of any window with the 'Esc' key
* Improved handling of update failures
* Fixed bug where changing text in dialogs didn't enable the 'OK' button
* Fixed bug where closing a Finder during a long search may cause a crash later
* Fixed automatic versioning code to remove dependency on VS extension
* Fixed many miscellaneous string and localization bugs
* Fixed Code Analysis warnings and issues to improve code quality and style
* Fixed Setup warnings
* Tidied XAML files for readability
* Performance and reliability improvements
* Updated Svg.NET to 2.3
* Updated MVVMLightLibs to 5.3.0.0

## 2.0.2 ##
* Chord/Scale Finder searches can now be canceled with the 'Esc' key
* Fixed bug with full barres in the Chord Finder crossing open/muted strings
* Fixed bug with partial barres in the Chord Finder not mirroring properly
* Fixed bug where reach wasn't calculating correctly in the Scale Finder
* Fixed bug where scales past the 12th fret weren't found in the Scale Finder
* Fixed bug where many scales were missed by the Scale Finder
* Added CoreTest unit testing project
* Added unit tests for finding where to barre in chords
* Refactored Chord/Scale Finder code for testability added some unit tests

## 2.0.1 ##
* Fixed bug where diagrams would render as black boxes in some locales
* Fixed bug where marks on the fretboard could be pushed off the bottom
when resizing
* Fixed bug where the nut was not being drawn in the correct position if
the nut ratio != 2.0
* Opacities and ratios are now presented as percentages
* Fixed various bugs with the formatting of decimal numbers
* Standardized install locations
* Renamed user config to Chordious.User.xml
* Added option to choose user config from the command-line
* Added option to hide/disable updating functionality

## 2.0 ##
* First 2.0 official release
* Fixed bugs in finders with missing instruments/tunings/intervals
* Fixed miscellaneous localization bugs

## 1.9.16292.1842 ##
* Added mark styles to the Diagram Style Editor
* Added mark styles to the Diagram Editor
* Updated Diagram Mark Editor
* Localized mark style strings
* Rearranged the groups in the Diagram Editor
* Some string updates

## 1.9.16276.349 ##
* Added fret label styles to the Diagram Style Editor
* Added fret label styles to the Diagram Editor
* Updated Diagram Fret Label Editor
* Localized fret label style strings
* Some string updates

## 1.9.16245.2247 ##
* Added barre styles to the Diagram Style Editor
* Added barre styles to the Diagram Editor
* Updated Diagram Barre Editor
* Localized barre style strings
* Some string updates

## 1.9.16171.2005 ##
* New diagram collection selector window
* Chord/scale finder now use collection selector
* Can now move/copy diagrams between collections in the library
* Rearranged diagram library context menus
* Some icon/string updates

## 1.9.16168.1613 ##
* Added buttons to reset the styles of diagrams
* Added buttons to reset the style in the style editor

## 1.9.16162.1448 ##
* Added license prompt to the installer
* Added option to start Chordious after install

## 1.9.16153.1831 ##
* Localized majority of remaining messages, windows, labels and tooltips
* Fixed bug with launching the style editor from Options
* Fixed bug with double-clicking on items in rows

## 1.9.16145.449 ##
* Added the new Diagram Style Editor with fully localized strings
* Updated the Diagram Editor to use the same setup of options as the Diagram Style Editor
* Added more tooltips to the Finders
* Localized update prompts and other strings

## 1.9.16121.2216 ##
* Updated config import/export to use resource strings
* Refactored DiagramStyle to support code re-use
* Fixed bug with collections not saving/loading their styles
* Added the start of the DiagramStyleEditor
* Fixed bug where a missing user config would throw an error on startup
* Added logic to catch if Chordious crashes during startup
* Added logic to catch corrupt user configs and give the user the option to create a backup of the corrupt config (hopefully to be debugged later) and reset Chordious to a fresh state

## 1.9.16103.221 ##
* Can now edit Finder results before saving them

## 1.9.16101.2003 ##
* Added new resource strings
* Updated ChordFinderWindow to use resource strings
* Updated ScaleFinderWindow to use resource strings

## 1.9.16092.503 ##
* Added option to remember some yes/no confirmations
* Fixed bug with true/false settings

## 1.9.16072.2038 ##
* New main icon!
* New color icons and button style changes
* Button icons now grey-out when disabled
* Added ability to clone diagrams within a library collection
* Added ability to scale exported image files (needs more testing)
* Removed SharpVectors and ability to choose SVG renderer
* Switched from Dynamic to Static resources to improve performance
* Fixed bug in options Window with reloading options after exiting the advanced editor
* Fixed bug in Diagram Export with filename case-sensitivity
* Fixed bug when trying to open/close files and the last path opened doesn't exist anymore
* Started moving hard-coded UI strings into resource files for future localization (incomplete)
* Started adding more tooltips to items (incomplete)

## 1.9.16062.348 ##
* Fixed bug with loading the diagram editor with a transparent render background

## 1.9.16061.1205 ##
* Resource images are now embedded
* Fixed copyright notice formatting

## 1.9.16059.2132 ##
* Can now easily create copies of collections in the Library
* Fixed crash when trying to edit the name of a Library collection and a diagram was previously selected
* Other minor bug-fixes

## 1.9.15354.202 ##
* Fixed a bug where the first time a user runs Chordious they get a STAThread error
* Other minor bug-fixes

## 1.9.15330.0833 ##
* Custom user instruments, tunings, chord qualities, and scales are all now clearly separated from the default built-in ones in editors
* Lists of such items are now all sorted alphabetically
* You can now have tunings, chord qualities, and scales with the same names but different other attributes (ie. two different Open G tunings, two different Dominant 11th variants)

## 1.9.15300.1631 ##
* Added prompt for barre width when adding barres (can now support shorter barres)
* Fixed bug where resizing diagram doesn't respect existing barres

## 1.9.15300.1608 ##
* New DiagramBarredEditor lets you customize barre styles in the DiagramEditor
* Added F5 to "refresh" the DiagramEditor
* Fixed multiple bugs related to creating/editing barres

## 1.9.15295.1615 ##
* Added F5 to "refresh" the Finders
* New DiagramFretLabelEditor lets you customize fret label styles in the DiagramEditor
* Fixed issues when selecting the wrong marks/fret labels in the editor
* Other minor bugfixes

## 1.9.15293.247 ##
* Added metadata to exported images
* Default config import/export is now the Desktop, and is now persisted
* Other bugfixes

## 1.9.15278.1540 ##
* New DiagramMarkEditor lets you customize mark styles in the DiagramEditor
* Launch via right-click menu or double-click
* Improved color selection (choose by name, 3-digit RGB or 6-digit RGB)
* Improved font selection (auto-complete and custom)
* Refactoring dropdown choices into ObservableEnums
* Better error handling in DiagramEditor
* Many core bugfixes and improvements around colors, marks, and fret labels

## 1.9.15265.239 ##
* Added better error messages for Chordious exceptions
* Removed all dependency on MessageBox.Show
* Added a new error dialog that exposes details for bug filing
* Added a new confirmation dialog
* Added a new information dialog

## 1.9.15252.1938 ##
* Implemented basic config import / export
* Moved legacy ChordLine import into Options
* Can now selectively load / save only parts of ConfigFiles

## 1.9.15250.507 ##
* Fixed issue #10, Chordious allows multiple instances, which means config file contention

## 1.9.15250.452 ##
* Fixed issue #9, chord qualities with repeating notes now work as expected
* Fixed issue #11, where Diagram Export overwrites files as they're made
* Added context menus and double-click to edit on items in the Library, Quality/Scale Managers, and the Instrument Manager

## 1.9.15244.326 ##
* Fixed issue #8, where the finders sometimes send the wrong quality/scale

## 1.9.15242.2314 ##
* Fixed ChordFinder to not assume the first note in a chord quality is the root
* Added a bunch of new chord qualities

## 1.9.15241.2118 ##
* Fixed issue #7, deselect diagrams when changing the selected library node
* Added button to add new diagrams to a collection directly
* Added options to specify the default strings and frets for new diagrams
* Fixed options window resizing and scroll bars
* Miscellaneous Core fixes

## 1.9.15232.1508 ##
* Fixed issue #3, exported image filenames are now cleaned before export
* Fixed issue #4, diagram export settings are remembered
* Fixed issue #6, missing directories are created when exporting images 

## 1.9.15230.1602 ##
* Fixed issue #5, where the chord finder would accidentally try to determine if a muted string was a root note

## 1.9.15227.235 ##
* Removed settings search defaults in the Options window
* Added buttons to clear search defaults in the Options window
* Added Diminished 7th and Augmented 7th chord qualities

## 1.9.15202.1600 ##
* Changing the default instruments / tunings in Options now sets the buffer as dirty so you can save the changes
* Can now save your search parameters in the Finders as the new defaults
* Can now reset the search parameters in the Finders to the defaults

## 1.9.15157.1749 ##
* For left/right diagrams, the mouse will now map to the correct coordinates
* Horizontally oriented diagrams now render mark text and fret labels properly

## 1.9.15125.1615 ##
* Added tooltip to explain export filename format
* Switched to dropdown to select from multiple filename formats

## 1.9.15123.2147 ##
* Added a new DiagramExport dialog

## 1.9.15110.221 ##
* Bug: Can't scroll lists of diagrams with the mouse wheel
* Replaced diagram edit button with double-click

## 1.9.15100.0 ##
* First 1.9 preview release
* Search for chords with the improved Chord Finder (supports custom chord types!)
* Search for scales with the new Scale Finder (supports custom scale types!)
* Add your own custom instruments and tunings
* Save and maintain your collections of diagrams in the new Diagram Library (incomplete)
* Import your Classic Chordious ChordLine documents
* Export your diagrams as SVG, JPG, or PNG images (incomplete)
* Rich Diagram Editor (incomplete)
* Quick and easy installer
* Built-in automatic updates!
