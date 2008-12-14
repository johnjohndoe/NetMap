
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
June 2007, most of the classes have not been implemented.


ApplicationUtil
---------------
Visual Studio project, contains classes useful to graphing applications.


Common
------
Contains classes that are used by two or more Visual Studio projects but not
contained in a shared assembly.  The classes are linked to the projects via the
Add as Link option in Visual Studio's Add Existing Item dialog.


Control
-------
Visual Studio project, contains a Windows Forms control that draws graphs.


Core
----
Visual Studio project, contains core classes representing graphs, vertices, and
edges.


DesktopApplication
------------------
Visual Studio project, contains a Windows Forms application that displays
graphs.  As of May 2008, further development of this project has been halted in
favor of its replacement, ExcelTemplate.  The application still works, however.


DesktopApplicationSetup
-----------------------
Visual Studio deployment project, installs DesktopApplication.


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


MultithreadingTests
-------------------
Visual Studio project, contains a Windows Forms application for testing
multithreaded classes that are too difficult to test with unit tests.


TestNodeXLControl
-----------------
Visual Studio project, contains a Windows Forms application for testing
the NodeXLControl.


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


Visualization
-------------
Visual Studio project, contains classes that lay out and draw graphs.


WpfControl
----------
Visual Studio project, contains a WPF control that draws graphs.


WpfVisualization
----------------
Visual Studio project, contains classes used to draw graphs in the WPF
environment.
