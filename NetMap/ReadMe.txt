
Overview
--------
NetMap is a network graphing API and a set of applications created by Marc
Smith's team at Microsoft Research.  To build NetMap, see
.\HowToBuildNetMap.txt.

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
Contains classes used by two or more Visual Studio projects.


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
NetMap documentation.


ExcelTemplate
-------------
Visual Studio project.  Contains a VSTO-based Excel 2007 document customization
that displays a graph within an Excel workbook.  This replaces
DesktopApplication.


MultithreadingTests
-------------------
Visual Studio project, contains a Windows Forms application for testing
multithreaded classes that are too difficult to test with unit tests.


TestNetMapControl
-----------------
Visual Studio project, contains a Windows Forms application for testing
the NetMapControl.


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
