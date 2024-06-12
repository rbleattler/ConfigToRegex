param(
  [switch]
  $skipBuild
)

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
$testPattern = Get-Content -Raw "$root\basicpattern.yml"
$groupPattern = Get-Content -Raw "$root\grouppattern.yml"
$groupPattern2 = Get-Content -Raw "$root\grouppattern2.yml"
$patternValue = Get-Content -Raw "$root\patternvalue.yml"
$testQuantifier = Get-Content -Raw "$root\quantifier.yml"
$patternProperties = Get-Content -Raw "$root\patternproperties.yml"
$anchorPattern = Get-Content -Raw "$root\anchorpattern.yml"

$pv = [RegexRules.PatternValue]::new($patternValue)
$q = [RegexRules.Quantifier]::new($testQuantifier)
$p = [RegexRules.Pattern]::new($testPattern)
$gp = [RegexRules.GroupPattern]::new($groupPattern)
$gp2 = [RegexRules.GroupPattern]::new($groupPattern2)
$pp = [RegexRules.PatternProperties]::new($patternProperties)
$ap = [RegexRules.AnchorPattern]::new($anchorPattern)

Write-Output "PatternValues:"
$pv | Format-List *
Write-Output "Quantifiers:"
$q | Format-List *
Write-Output "Patterns:"
$p | Format-List *
Write-Output "Group Patterns:"
$gp | Format-List *
$gp2 | Format-List *
Write-Output "Pattern Properties:"
$pp | Format-List *
Write-Output "Anchor Patterns:"
$ap | Format-List *