<#
Sets version attributes in all NodeXL-related projects.
#>

function
GetFileInfos
(
    [String]$sFullFolderName
)
{
    return get-childitem $sFullFolderName -recurse -include "AssemblyInfo.cs"
}

function GetFileContentAsString
(
    [String]$sFullFileName
)
{
	return ( [String]::Join( [environment]::newline, `
		(Get-Content -path $sFullFileName) ) )
}

function WriteStringToFile
(
    [String]$sString,
    [String]$sFullFileName
)
{
	if ( (get-childitem $sFullFileName).IsReadOnly )
	{
		Write-Host $oFileInfo.FullName "is read-only." -foregroundcolor Red
		return
	}

	$sString | Out-File -filepath $sFullFileName -Encoding UTF8
}

function RegexReplace
(
    [String]$sInput,
    [String]$sPattern,
    [String]$sReplacement
)
{
	[System.Text.RegularExpressions.Regex]$oRegex = New-Object `
		-TypeName System.Text.RegularExpressions.Regex `
		-ArgumentList $sPattern

	return ( $oRegex.Replace($sInput, $sReplacement) )
}

function ProcessAssemblyInfoFile
(
    [String]$sFullFileName,
    [Int]$iRevision
)
{
	[String]$sFileContents = GetFileContentAsString($sFullFileName)

	[String]$sPattern1 = 'AssemblyVersion\("\d+\.\d+\.\d+\.\d+"\)'
	[String]$sReplacement1 = 'AssemblyVersion("1.0.1.' + $iRevision + '")'

	[String]$sPattern2 = 'AssemblyFileVersion\("\d+\.\d+\.\d+\.\d+"\)'
	[String]$sReplacement2 = 'AssemblyFileVersion("1.0.1.' + $iRevision + '")'

	$sFileContents = RegexReplace $sFileContents $sPattern1 $sReplacement1
	$sFileContents = RegexReplace $sFileContents $sPattern2 $sReplacement2

	WriteStringToFile $sFileContents $sFullFileName
}

function ProcessAssemblyInfoFiles
(
    [String]$sFullFolderName,
    [Int]$iRevision
)
{
    foreach ( $oFileInfo in `
		get-childitem $sFullFolderName -recurse -include "AssemblyInfo.cs")
    {
        ProcessAssemblyInfoFile $oFileInfo.FullName $iRevision
    }
}

function ProcessExcelTemplateCsprojFile
(
    [String]$sPathRoot,
    [Int]$iRevision
)
{
	[String]$sFullFileName = `
		$sPathRoot + "NodeXL\ExcelTemplate\ExcelTemplate.csproj"

	[String]$sFileContents = GetFileContentAsString($sFullFileName)
	[String]$sPattern = '<ApplicationVersion>\d+\.\d+\.\d+\.\d+'
	[String]$sReplacement = '<ApplicationVersion>1.0.1.' + $iRevision

	$sFileContents = RegexReplace $sFileContents $sPattern $sReplacement

	WriteStringToFile $sFileContents $sFullFileName
}

function Main()
{
    if ($args.count -ne 1)
    {
        Write-Host "Usage: SetAssemblyVersion Revision" -foregroundcolor Red
        Exit
    }
    
    [Int]$iRevision = $args[0]
    [String]$sPathRoot = "E:\"
    
    ProcessAssemblyInfoFiles ($sPathRoot + "NodeXL") $iRevision

	ProcessExcelTemplateCsprojFile $sPathRoot $iRevision
}

Main(150)
