
Overview
--------
NodeXL is a network graphing API and a set of applications created by Marc
Smith's team at Microsoft Research.  To build NodeXL, see
.\HowToBuildNodeXL.txt.

The following sections describe the subdirectories within this directory.


Adapters
--------
Visual Studio project, contains graph adapters that read and write graphs to
various file formats.


Algorithms
----------
Visual Studio project, contains classes that implement graph algorithms.  As of
June 2007, most of the classes have not been implemented.  Graph algorithms are
implemented in ExcelTemplate, which is the wrong place.  ExcelTemplate's graph
algorithms need to be refactored to make them available to other applications.


ApplicationUtil
---------------
Visual Studio project, contains classes useful to graphing applications.


Common
------
Contains classes that are used by two or more Visual Studio projects but not
contained in a shared assembly.  The classes are linked to the projects via the
Add as Link option in Visual Studio's Add Existing Item dialog.


Core
----
Visual Studio project, contains core classes representing graphs, vertices, and
edges.


Documents
---------
NodeXL documentation.


ExcelTemplate
-------------
Visual Studio project.  Contains a VSTO-based Excel 2007 document customization
that displays a graph within an Excel workbook.  This replaces
DesktopApplication.


ExcelTemplateRegisterUser
-------------------------
Visual Studio project, contains a Windows Forms application that registers a
user of the ExcelTemplate.  This gets run from within ExcelTemplateSetup.


ExcelTemplateSetup
------------------
Visual Studio deployment project, installs ExcelTemplate.


ExcelTemplateSetupCustomActions
-------------------------------
Custom action classes used by ExcelTemplateSetup.


Layouts
-------------
Visual Studio project, contains classes that lay out graphs.


TestWpfNodeXLControl
--------------------
Visual Studio project, contains a WPF application for testing the WPF
NodeXLControl.


UnitTests
---------
Visual Studio project, contains unit tests for testing classes in the other
projects.


Util
----
Visual Studio project, contains classes used by two or more Visual Studio
projects.


WpfControl
----------
Visual Studio project, contains a WPF control that draws graphs.


WpfVisualization
----------------
Visual Studio project, contains classes used to draw graphs in the WPF
environment.
