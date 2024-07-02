#requires -Version 6.0
using namespace System.Collections.Generic;

[CmdletBinding(DefaultParameterSetName = 'Default')]
param(
  [Parameter(ParameterSetName = 'Package')]
  [string]
  $Version = '0.0.1',
  [Parameter(ParameterSetName = 'Default')]
  [Parameter(ParameterSetName = 'Package')]
  [ValidateSet('Debug', 'Release')]
  [string]
  $Configuration = 'Release',
  [Parameter()]
  [Parameter(ParameterSetName = 'Default')]
  [Parameter(ParameterSetName = 'Package')]
  [string]
  $ProjectName = "ConfigToRegex",
  [Parameter(ParameterSetName = 'Default')]
  [Parameter(ParameterSetName = 'Package')]
  [ValidateSet('Clean', 'Restore', 'Build', 'Test', 'Publish', 'Pack')]
  [List[string]]
  $Steps = @( 'Clean', 'Restore', 'Build', 'Test', 'Publish', 'Pack'),
  [Parameter(ParameterSetName = 'Default')]
  [Parameter(ParameterSetName = 'Package')]
  [string]
  $OutputDirectory = "$PSScriptRoot\..\scratch"
)
begin {
  Write-Debug "[Begin]:[LocalBuild.ps1]:[Enter]"
  function Invoke-LocalBuild {
    [CmdletBinding()]
    param(
      [string]
      $Version = '0.0.1',
      [switch]
      $Pack,
      [string]
      $Configuration = 'Release',
      [string]
      $ProjectName = "ConfigToRegex",
      [List[string]]
      $Steps
    )
    Write-Verbose "[Invoke-LocalBuild]:: Steps: $($Steps -join ', ')"
    if ($ProjectName -ne 'ConfigToRegex') {
      Write-Warning "ProjectName is not set. Defaulting to 'ConfigToRegex'"
      $ProjectName = "ConfigToRegex"
    }
    switch ($Steps) {
      'Clean' {
        Write-Host "Cleaning project..." -ForegroundColor Yellow &&
        Write-Verbose "[CLEAN]"
        dotnet clean
      }
      'Restore' {
        Write-Host "Restoring packages..." -ForegroundColor Yellow &&
        Write-Verbose "[RESTORE]"
        dotnet restore
      }
      'Build' {
        Write-Host "Building version $Version..." -ForegroundColor Yellow &&
        Write-Verbose "[BUILD]"
        dotnet build --configuration $Configuration
      }
      'Test' {
        Write-Host "Running tests..." -ForegroundColor Yellow &&
        Write-Verbose "[TEST]"
        dotnet test
      }
      'Pack' {
        Write-Host "Packaging..." -ForegroundColor Yellow &&
        Write-Verbose "[PACK]"
        Write-Debug "dotnet pack $PSScriptRoot\..\src\$ProjectName\$ProjectName.csproj --output $OutputDirectory -p:NuspecFile=$PSScriptRoot\..\src\$ProjectName\$ProjectName.nuspec -p:version=`"$($Version)`" /p:PackageVersion=`"$($Version)`" /p:PackageReleaseNotes=`"Local build`" /p:PackageTags=`"local`" /p:NugetVersion=`"$($Version)-local`""
        dotnet pack $PSScriptRoot\..\src\$ProjectName\$ProjectName.csproj --output $OutputDirectory -p:NuspecFile=$PSScriptRoot\..\src\$ProjectName\$ProjectName.nuspec -p:version="$($Version)" /p:PackageVersion="$($Version)" /p:PackageReleaseNotes="Local build" /p:PackageTags="local" /p:NugetVersion="$($Version)-local"
      }
      'Publish' {
        Write-Host "Publishing..." -ForegroundColor Yellow &&
        Write-Verbose "[PUBLISH]"
        Write-Debug "dotnet publish $PSScriptRoot\..\src\$ProjectName\$ProjectName.csproj --configuration $Configuration /p:version=`"$Version`" /p:PackageVersion=`"$Version`" /p:InformationalVersion=`"$Version-local`""
        dotnet publish $PSScriptRoot\..\src\$ProjectName\$ProjectName.csproj --configuration $Configuration /p:version="$Version" /p:PackageVersion="$Version" /p:InformationalVersion="$Version-local"
      }
      default {
        Write-Warning "I'm not sure what you're trying to do here... But we're going to assume it's 'Everything' and start over..."
        Invoke-LocalBuild -Version $Version -Pack $Pack -Configuration $Configuration -Steps @( 'Clean', 'Restore', 'Build', 'Test', 'Publish', 'Pack' )
      }
    }
  }
  if ($Steps.Count -eq 0) {
    Write-Warning "No steps were provided. Defaulting to 'Everything'..."
    $PSBoundParameters["Steps"] = @( 'Clean', 'Restore', 'Build', 'Test', 'Publish', 'Pack' )
    Write-Debug 'Pack' -in $PSBoundParameters["Steps"]
    # $Steps = @( 'Clean', 'Restore', 'Build', 'Test', 'Publish','Pack' )
  }
  Write-Debug "[Begin][LocalBuild.ps1][Exit]"
}
process {
  Write-Debug "[Process][LocalBuild.ps1][Enter]"

  Write-Verbose "ProjectName: $ProjectName"
  Write-Verbose "Version: $Version"
  Write-Verbose "Steps: $($Steps -join ', ')"
  Write-Verbose "Configuration: $Configuration"
  Invoke-LocalBuild @PSBoundParameters

  Write-Debug "[Process][LocalBuild.ps1][Exit]"
}
end {
  Write-Debug "[End][LocalBuild.ps1][Enter]"
  Write-Debug "[End][LocalBuild.ps1][Exit]"
}