#! /usr/bin/env pwsh

Param(
    [string]$Configuration = "Release",
    [switch]$Interactive
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

Write-Host "Running benchmarks..."

$additionalArgs = @()

if ($Interactive -ne $true) {
    $additionalArgs += "--"
    $additionalArgs += "--filter"
    $additionalArgs += "*"
}

$project = Join-Path "src" "Polly.Core.Benchmarks" "Polly.Core.Benchmarks.csproj"

dotnet run --configuration $Configuration --framework net7.0 --project $project $additionalArgs

exit $LASTEXITCODE
