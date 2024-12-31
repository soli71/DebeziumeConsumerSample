$ErrorActionPreference = "Stop"

$idlPath = "./Avros"

$rootPath = "./_Records"
$outputRoot = "$rootPath"
$schemaRoot = "$outputRoot/schema"
$classesRoot = "$outputRoot/imp"

# Store tools outside the repo (saves repeated downloads)
$avroToolsUri = "https://repo1.maven.org/maven2/org/apache/avro/avro-tools/1.10.0/avro-tools-1.10.0.jar"
$avroToolsRoot = "$HOME/.avro-tools"
$avroToolsPath = "$avroToolsRoot/avro-tools-1.10.0.jar"

# Where avrogen is installed to (doing it this way works within a docker container)
$avrogenPath = "$HOME/.dotnet/tools/avrogen"

$runBuild = $false

# Do we need to run this?
if (Test-Path -Path $outputRoot)
{
    $newestClass = Get-ChildItem -Path $outputRoot -Recurse | Sort-Object LastWriteTime | Select -Last 1
    $newestIdl = Get-ChildItem -Path $idlPath -Recurse -Filter "*.avdl" | Sort-Object LastWriteTime | Select -Last 1

    if ($newestIdl.LastWriteTime -gt $newestClass.LastWriteTime)
    {
        Write-Output "AVDL change detected, regenerating..."
        Remove-Item -Recurse $outputRoot
        $runBuild = $true
    }
    else
    {
        Write-Output "No AVDL changes detected"
    }
}
else
{
    Write-Output "Generating Avro classes..."
    $runBuild = $true
}

if ($runBuild)
{
    # Create the folder if it doesn't exist
    if (-Not (Test-Path -Path $rootPath))
    {
        New-Item -Path $rootPath -ItemType Directory | Out-Null
    }

    # Download avro-tools if it can't find it
    if (-Not (Test-Path -Path $avroToolsPath))
    {
        New-Item -Path $avroToolsRoot -ItemType Directory -Force | Out-Null
        Write-Output "avro-tools not found, downloading from $avroToolsUri to $avroToolsPath"
        Invoke-WebRequest -Uri $avroToolsUri -OutFile $avroToolsPath
    }

    $inputIdls = Get-ChildItem -Path $idlPath -Recurse -Filter "*.avdl" | Resolve-Path -Relative

    foreach ($inputIdl in $inputIdls)
    {
        $schemaOutputPath = $schemaRoot + (Split-Path -Path $inputIdl).Replace($idlPath, "")

        $command = "java -jar $avroToolsPath idl2schemata $inputIdl $schemaOutputPath"

        Write-Output "Running: $command"
        Invoke-Expression -Command $command
    }

    $inputSchemas = Get-ChildItem -Path $schemaRoot -Recurse -Filter "*.avsc" | Resolve-Path -Relative

    foreach ($inputSchema in $inputSchemas)
    {
        $command = "$avrogenPath -s $inputSchema $classesRoot"

        Write-Output "Running: $command"
        Invoke-Expression -Command $command
    }
}