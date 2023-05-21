$SSRSInstallDir = "C:\Program Files\Microsoft Sql Server Reporting Services\SSRS"
$deployDestinationDirs = "$SSRSInstallDir\ReportServer\Bin\", "$SSRSInstallDir\Portal"
$saturnSourceCodeOutputDir = "c:\repos\saturn\artifacts\bin\debug\"

$reportServer_BackupDir = "$SSRSInstallDir\ReportServer\.bak\"
New-Item -ItemType Directory -Path $reportServer_BackupDir -Force

#Backup files to .bak dir
$filesToCopy = @("Logon.aspx")
foreach ($fileName in $filesToCopy) {
    Write-Host "Deploying $fileName`n" -ForegroundColor Green

    $existingFile = "$SSRSInstallDir\ReportServer\$fileName";
    if (Test-Path $existingFile) {
        Move-Item -Path "$SSRSInstallDir\ReportServer\$fileName" -Destination "$reportServer_BackupDir\$fileName.$(get-date -f yyyyddMMhhmmss).bak"
    }

    Copy-Item -Path ".\$fileName" -Destination "$SSRSInstallDir\ReportServer\" -Force
}

Write-Host "Deploying Saturn.SSRSSecurityExtension library and dependencies`n" -ForegroundColor Green
foreach ($d in $deployDestinationDirs) {
    Copy-Item -Path "$saturnSourceCodeOutputDir\Saturn.SSRSSecurityExtension.*" -Destination $d
    Copy-Item -Path "$saturnSourceCodeOutputDir\Saturn.SSRSSecurityExtension.*" -Destination $d
    Copy-Item -Path "$saturnSourceCodeOutputDir\Saturn.Security.Common.*" -Destination $d

    Copy-Item -Path "$saturnSourceCodeOutputDir\authservice\net7.0\System.IdentityModel.Tokens.Jwt.dll" -Destination $d
    Copy-Item -Path "$saturnSourceCodeOutputDir\authservice\net7.0\Microsoft.IdentityModel.Tokens.dll" -Destination $

    Copy-Item -Path "$saturnSourceCodeOutputDir\authservice\net7.0\Microsoft.IdentityModel.Logging.dll" -Destination $d
    Copy-Item -Path "$saturnSourceCodeOutputDir\authservice\net7.0\Microsoft.IdentityModel.JsonWebTokens.dll" -Destination $d
    Copy-Item -Path "$saturnSourceCodeOutputDir\authservice\net7.0\System.Security.Permissions.dll" -Destination $d
}

#SSRS configuration files

#Merge old and new files if possible and necessary
