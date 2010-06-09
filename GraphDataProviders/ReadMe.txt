
Overview
--------
This solution contains projects that get graph data for NodeXL, which is an
Excel template for displaying and analyzing network graphs.  NodeXL itself is
implemented in another solution.

The following sections describe the subdirectories within this directory,
listed in alphabetical order.


GraphDataProviders
------------------
Class library.  Contains several classes that implement the
Microsoft.NodeXL.ExcelTemplatePlugIns.IGraphDataProvider interface.   These
classes are plugins for the NodeXL Excel template.  They import graph data into
the template from data sources that the template doesn't know about.

See the IGraphDataProvider interface for details.


NetworkServer
-------------
Console application.  Gets graph data and stores it in a variety of file
formats.


TestGraphDataProviders
----------------------
Windows Forms application.  Tests the IGraphDataProvider classes in
GraphDataProviders.


UnitTests
---------
Unit tests.  Tests some of the classes contained in the other projects.
