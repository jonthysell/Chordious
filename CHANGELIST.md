# Chordious Changelist #

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
