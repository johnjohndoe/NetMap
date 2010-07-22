
Overview
--------
NodeXL is an Excel 2007 template for displaying and analyzing network graphs,
along with an API for incorporating graphs in other applications.  To build
NodeXL, see .\HowToBuildNodeXL.txt.

The following sections describe the subdirectories within this directory,
listed in alphabetical order.


Adapters
--------
Class library project.  Contains graph adapters that read and write graphs to
various file formats.


Algorithms
----------
Class library project.  Contains classes that implement graph algorithms.


ApplicationUtil
---------------
Class library project.  Contains classes useful to graphing applications,
including ExcelTemplate.


BuildTools
----------
Tools needed by the build process, but not needed on client machines.


Common
------
Classes that are used by two or more projects but not compiled into a shared
assembly.  The classes are linked to the projects via the Add as Link option in
Visual Studio's Add Existing Item dialog.


Core
----
Class library project.  Contains core classes representing graphs, vertices,
and edges.


Documents
---------
NodeXL documentation.


ExcelTemplate
-------------
VSTO-based Excel 2007 template project that displays a graph within an Excel
workbook.


ExcelTemplatePlugIns
--------------------
Class library project.  Contains interface definitions for plug-ins used by
ExcelTemplate.


ExcelTemplateRegisterUser
-------------------------
Windows Forms project.  Registers a user of the ExcelTemplate from within
ExcelTemplateSetup and ExcelTemplate.


ExcelTemplateSetup
------------------
Deployment project.  Installs ExcelTemplate.


ExcelTemplateSetupClickOnceInstaller
------------------------------------
Class library project.  Contains custom action class used by
ExcelTemplateSetup.


ExcelTemplateSetupStarter
-------------------------
Console project.  Starts the Excel Template setup process on a client machine.


ExcelTemplateSetupTrustInstaller
--------------------------------
Class library project.  Contains custom action class used by
ExcelTemplateSetup.


GraphDataProviders
------------------
Class library project.  Contains several classes that implement the
Microsoft.NodeXL.ExcelTemplatePlugIns.IGraphDataProvider interface.   These
classes are plugins for the ExcelTemplate project.  They import graph data into
the template from data sources that the template doesn't know about, such as
Twitter, YouTube, and Flickr.


Layouts
-------------
Class library project.  Contains classes that lay out graphs.


NetworkServer
-------------
Console project.  Gets graph data using the classes in GraphDataProviders and
stores it in a variety of file formats.  This is a console-based alternative to
getting the same graph data from within the ExcelTemplate.


TestGraphDataProviders
----------------------
Windows Forms project.  Tests the IGraphDataProvider classes in
GraphDataProviders.


TestWpfNodeXLControl
--------------------
Windows Forms project.  Tests the NodeXLControl.


UnitTests
---------
Unit test project.  Tests classes in the other projects.


Util
----
Class library project.  Contains classes used by two or more of the other
projects.


WpfControl
----------
WPF custom control library project.  Contains NodeXLControl, a WPF control that
draws graphs.


WpfVisualization
----------------
Class library project.  Contains classes used to draw graphs in the WPF
environment.
