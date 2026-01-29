$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectPath = Join-Path $scriptDir "..\ModernGridViewCrud\ModernGridViewCrud.csproj"
$outputDir = Join-Path $scriptDir "..\bin\Release\net8.0\linux-x64\publish"

Write-Host "Publishing ModernGridViewCrud for Linux..."
dotnet publish $projectPath -c Release -r linux-x64 --self-contained false -o $outputDir

if ($LASTEXITCODE -eq 0) {
    Write-Host "Publish successful! Artifacts are in $outputDir"
} else {
    Write-Host "Publish failed."
    exit 1
}
