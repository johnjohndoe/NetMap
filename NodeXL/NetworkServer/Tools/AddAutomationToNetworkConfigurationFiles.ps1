
$sUsage = @"
For each NodeXL Network Server network configuration file in a specified
folder, this PowerShell script adds <AutomateNodeXLWorkbook> elements to the
file and sets the elements' values to true.  This can be used to update network
configuration files that were created for earlier versions of the NodeXL
Network Server that did not support NodeXL workbook automation.

Sample usage:

PowerShell .\AddAutomationToNetworkCOnfigurationFiles "C:\Configuration Files"
"@

Write-Host ""

if ($args.Length -ne 1)
{

    Write-Host $sUsage
    Exit
}

[String]$sFolder = $args[0]

if (-not [System.IO.Directory]::Exists($sFolder) )
{
	Write-Host ("The folder " + $sFolder + " doesn't exist.") `
		-ForegroundColor red

	Exit
}


$sAutomateNodeXLWorkbookText = @"

        <!--
        ***********************************************************************
        AutomateNodeXLWorkbook
        
        Specifies whether the NodeXL Excel Template's automate feature should
        be run on the workbook.  Must be true or false.  This is used only if
        NetworkFileFormats (above) includes NodeXLWorkbook.

        If true, the automate options you most recently set in the NodeXL Excel
        Template are used to automate the workbook.  To set the automate
        options, do the following:

            1. Open the NodeXL Excel Template.

            2. In the Excel ribbon, Go to NodeXL, Graph, Automate.

        Note that the "On this workbook" and "On every NodeXL workbook in this
        folder" selection in the Automate dialog box is ignored when automating
        the workbook from the NodeXL Network Server.
        ***********************************************************************
        -->>

        <AutomateNodeXLWorkbook>true</AutomateNodeXLWorkbook>

"@

foreach ($oXmlFileInfo in get-childitem ($sFolder + "\*") -include *.xml)
{
	[System.Xml.XmlDocument]$oXmlDocument = `
		New-Object -TypeName System.Xml.XmlDocument

	[String]$sFilePath = $oXmlFileInfo.FullName

	try
	{
		$oXmlDocument.Load($sFilePath)
	}
	catch [System.Exception]
	{
		Write-Host ("Skipping " + $sFilePath `
			+ ", which is not a valid XML file.")

		continue
	}

	[String[]]$asInsertionPoints = `
		"</TwitterSearchNetworkConfiguration>",
		"</TwitterUserNetworkConfiguration>"

	Write-Host "Adding automation to" $sFilePath

	foreach ($sInsertionPoint in $asInsertionPoints)
	{
		(Get-Content $sFilePath) | 

			Foreach-Object {$_ -replace $sInsertionPoint, `
				($sAutomateNodeXLWorkbookText + "`n" + $sInsertionPoint)} | 

			Set-Content $sFilePath
	}
}
