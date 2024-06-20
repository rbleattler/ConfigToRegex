using namespace System.Collections.Generic;
[CmdletBinding()]
param(
  [switch]
  $skipBuild,
  [switch]
  $iterative
)
$DebugPreference = 'Continue'
$VerbosePreference = 'Continue'
$ErrorActionPreference = 'Stop'


$root = $PSScriptRoot

Push-Location $PSScriptRoot\..\..\..\
if (-not $skipBuild) {
  dotnet clean && dotnet build --configuration release && dotnet publish &&
  Import-Module .\src\RegexRules\bin\Release\net8.0\publish\RegexRules.dll
} else {
  try {
    Import-Module .\src\RegexRules\bin\Release\net8.0\publish\RegexRules.dll
  } catch {
    Write-Warning "Please run the script without the -skipBuild flag first"
  }
}
Pop-Location

if ($iterative) {


  #region iterative

  $vars = [List[PSObject]]::new()
  foreach ($file in Get-ChildItem -Path $root -Filter *.yml) {
    $content = Get-Content -Raw $file.FullName
    $name = $file.Name -replace '\.yml$'
    $v = New-Variable -Name $name -Value $content -Force -PassThru
    $vars.Add($v)
  }

  function New-RegexRulesObject {
    [CmdletBinding()]
    param(
      [string]$name,
      [string]$content
    )

    Write-Debug "Creating object from: $($name).yml"
    Write-Debug "Content:`n`n$($content)"
    $type = switch ($name) {
      # if the name is *like* *basicpattern, then create a new Pattern object
      { $_ -like "basicpattern*" } {
        Write-Debug "Matched basicpattern*"
        'RegexRules.Pattern'
      }
      { $_ -like "groupPattern*" } {
        Write-Debug "Matched groupPattern"
        'RegexRules.GroupPattern'
        break
      }
      { $_ -like "quantifier*" } {
        Write-Debug "Matched quantifier"
        'RegexRules.Quantifier'
        break
      }
      { $_ -like "patternValue*" } {
        Write-Debug "Matched patternValue"
        'RegexRules.PatternValue'
        break
      }
      { $_ -like "patternProperties*" } {
        Write-Debug "Matched patternProperties"
        'RegexRules.PatternProperties'
        break
      }
      { $_ -like "anchorPattern*" } {
        Write-Debug "Matched anchorPattern"
        'RegexRules.AnchorPattern'
        break
      }
      default {
        Write-Debug "Default case - no match"
        Write-Debug "Name: $($name)"
        Write-Debug "Content:`n`n$($content)`n"
        $skip = $true
        $null
        break
      }
    }

    # is this a collection?
    if ($content -match '^\s*-\s') {
      Write-Debug "Matched collection"
      $type = "System.Collections.Generic.List[$type]"
      $tempObject = New-Object -TypeName $type
      $type = $tempObject.GetType()
      #[YamlDotNet.Serialization.Deserializer]::new().Deserialize(
    }

    if ($skip) {
      Write-Debug "Skipping object creation"
      return
    }
    Write-Debug "Creating object of type: $($type)"
    try {
      $obj = [YamlDotNet.Serialization.Deserializer]::new().Deserialize($content, $type)
      # $obj = New-Object -TypeName $type -ArgumentList $content
    } catch {
      Write-Warning "Failed to create object of type: $($type) from file: $($name).yml"
      Write-Warning $PSItem.Exception.Message
      return
    }
    return $obj
  }

  # $out = [List[object]]::new()
  $out = [Dictionary[[string], [object]]]::new()
  $vars | ForEach-Object {
    # $v = $_
    # $typeOfObject = $PSItem.Value.GetType().Name
    Write-Host "`n"
    Write-Verbose "---"
    Write-Verbose "Processing: $($PSItem.Name)"
    Write-Verbose "---"
    Write-Host "`n"
    $obj = New-RegexRulesObject -name $PSItem.Name -content $PSItem.Value
    if ($null -ne $obj) {
      Write-Debug "Object created: Type - $($Obj.GetType().Name)"
      # $out.Add($obj)
      $out.Add($PSItem.Name, $obj)
    } else {
      Write-Warning "Failed to create object from file: $($PSItem.Name).yml"
    }
    Write-Host "`n"
    Write-Verbose "---"
    Write-Verbose "Finished processing: $($PSItem.Name)"
    Write-Verbose "---"
    # if ($out.Count -ge 6) {
    #   break
    # }
  }

  # $out
  #endregion
}


# $testPattern = Get-Content -Raw "$root\basicpattern.yml"
# # $testPattern2 = Get-Content -Raw "$root\basicpattern2.yml"
# $groupPattern = Get-Content -Raw "$root\grouppattern.yml"
# $groupPattern2 = Get-Content -Raw "$root\grouppattern2.yml"
# $patternValue = Get-Content -Raw "$root\patternvalue.yml"
# $testQuantifier = Get-Content -Raw "$root\quantifier.yml"
# $patternProperties = Get-Content -Raw "$root\patternproperties.yml"
# $anchorPattern = Get-Content -Raw "$root\anchorpattern.yml"

# $pv = [RegexRules.PatternValue]::new($patternValue)
# $q = [RegexRules.Quantifier]::new($testQuantifier)
# $p = [RegexRules.Pattern]::new($testPattern)
# # $p2 = [List[RegexRules.Pattern]]::new($testPattern2)
# $gp = [RegexRules.GroupPattern]::new($groupPattern)
# $gp2 = [RegexRules.GroupPattern]::new($groupPattern2)
# $pp = [RegexRules.PatternProperties]::new($patternProperties)
# $ap = [RegexRules.AnchorPattern]::new($anchorPattern)

# Write-Output "Patterns:"
# $p | Format-List *
# $p2 | Format-List *
# Write-Output "PatternValues:"
# $pv | Format-List *
# Write-Output "Quantifiers:"
# $q | Format-List *
# Write-Output "Group Patterns:"
# $gp | Format-List *
# $gp2 | Format-List *
# Write-Output "Pattern Properties:"
# $pp | Format-List *
# Write-Output "Anchor Patterns:"
# $ap | Format-List *