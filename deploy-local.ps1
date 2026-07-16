# ====================================================================
# SKRIP OTOMATISASI CI/CD LOKAL - WINDOWS SERVER (IIS)
# ====================================================================

# --- KONFIGURASI PATH ---
$SourceFolder  = "C:\Users\ICT\Documents\testdso-master"
$PublishFolder = "$SourceFolder\Prove.API\bin\Release\netcoreapp3.1\publish"
$IISFolder     = "C:\inetpub\wwwroot\testdso-app"
$AppPoolName   = "DefaultAppPool" # Sesuaikan dengan App Pool yang Anda gunakan di IIS
# ------------------------

Clear-Host
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "          MEMULAI PIPELINE CI/CD LOKAL" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan

# 1. TAHAP CI: Melakukan Build Otomatis
Write-Host "[1/4] TAHAP CI: Memulai Kompilasi (Publish) Proyek..." -ForegroundColor Yellow
cd $SourceFolder
dotnet publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Proses Kompilasi/CI Gagal!" -ForegroundColor Red
    Exit
}
Write-Host "✔ Kompilasi Berhasil!" -ForegroundColor Green

# 2. TAHAP CD: Hentikan App Pool IIS Sementara (Agar file .dll tidak terkunci saat ditimpa)
Write-Host "[2/4] TAHAP CD: Menghentikan Application Pool IIS..." -ForegroundColor Yellow
Import-Module WebAdministration
Stop-WebAppPool -Name $AppPoolName
Start-Sleep -Seconds 2

# 3. TAHAP CD: Menyalin File Rilis & Amankan appsettings.json
Write-Host "[3/4] TAHAP CD: Memperbarui File di Folder IIS..." -ForegroundColor Yellow

# Membuat folder IIS jika belum ada
if (!(Test-Path $IISFolder)) { New-Item -Path $IISFolder -ItemType Directory | Out-Null }

# Hapus file lama KECUALI appsettings.json agar konfigurasi database Anda tidak hilang
Get-ChildItem -Path $IISFolder -Exclude "appsettings*.json" | Remove-Item -Recurse -Force

# Menyalin hasil build baru ke folder aktif IIS
Copy-Item -Path "$PublishFolder\*" -Destination $IISFolder -Recurse -Force
Write-Host "✔ File Berhasil Diperbarui!" -ForegroundColor Green

# 4. TAHAP CD: Menyalakan Kembali Website
Write-Host "[4/4] TAHAP CD: Menyalakan Kembali Application Pool IIS..." -ForegroundColor Yellow
Start-WebAppPool -Name $AppPoolName

Write-Host "==================================================" -ForegroundColor Green
Write-Host "     SUKSES! PIPELINE CI/CD SELESAI DIJALANKAN" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green
