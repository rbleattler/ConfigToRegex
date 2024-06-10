param(
  [switch]
  $skipBuild
)

$root = $PSScriptRoot

if (-not $skipBuild) {
  Set-Location $PSScriptRoot\..\ && dotnet clean && dotnet build --configuration release && dotnet publish &&
  Import-Module .\src\RegexRules\bin\Release\net8.0\publish\RegexRules.dll
} else {
  try { Import-Module .\src\RegexRules\bin\Release\net8.0\publish\RegexRules.dll }catch {
    Write-Warning "Please run the script without the -skipBuild flag first"
  }
}
$testPattern = Get-Content -Raw "$root\basicpattern.yml"
$groupPattern = Get-Content -Raw "$root\grouppattern.yml"
$groupPattern2 = Get-Content -Raw "$root\grouppattern2.yml"
$patternValue = Get-Content -Raw "$root\patternvalue.yml"
$testQuantifier = Get-Content -Raw "$root\quantifier.yml"

$pv = [RegexRules.PatternValue]::new($patternValue)
$q = [RegexRules.Quantifier]::new($testQuantifier)
$p = [RegexRules.Pattern]::new($testPattern)
$gp = [RegexRules.GroupPattern]::new($groupPattern)
$gp2 = [RegexRules.GroupPattern]::new($groupPattern2)

Write-Output "PatternValues:"
$pv | Format-List *
Write-Output "Quantifiers:"
$q | Format-List *
Write-Output "Patterns:"
$p | Format-List *
Write-Output "Group Patterns:"
$gp | Format-List *
$gp2 | Format-List *