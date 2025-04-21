@echo off
echo ===== Starting Codefire C# (.NET) Setup for Windows =====

:: Navigate to project root (regardless of where the script is run from)
set "SCRIPT_DIR=%~dp0"
set "PROJECT_DIR=%SCRIPT_DIR%.."
cd /d "%PROJECT_DIR%"
echo Working directory: %CD%

:: Check if .NET SDK is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo .NET SDK is not installed. Please install .NET SDK before proceeding.
    echo Visit https://dotnet.microsoft.com/download to download and install .NET SDK.
    
    :: Try to open the download page
    start "" "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    
    exit /b 1
)

:: Display .NET version
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo .NET SDK version detected: %DOTNET_VERSION%

:: Extract major version number
for /f "tokens=1 delims=." %%a in ("%DOTNET_VERSION%") do set DOTNET_MAJOR_VERSION=%%a

:: Check for .NET 8
if %DOTNET_MAJOR_VERSION% LSS 8 (
    echo This project requires .NET 8.0 or higher. You have .NET %DOTNET_VERSION% installed.
    echo Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
    
    :: Try to open the download page
    start "" "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
    
    :: Ask if user wants to continue anyway
    set /p CONTINUE=Do you want to continue anyway? This may not work correctly. (y/n): 
    if /i "%CONTINUE%" neq "y" exit /b 1
)

echo ===== Restoring NuGet packages =====
dotnet restore

echo ===== Building solution =====
dotnet build --no-restore

echo ===== Running tests =====
dotnet test --no-build

echo ===== Testing the API setup =====
echo Attempting to start the API for 5 seconds to test the setup...
echo This window will close automatically after 5 seconds...

:: Change directory to the API project
cd src\CodeFire.API

:: Start API and kill after 5 seconds
start /b cmd /c "dotnet run --no-build & timeout /t 5 & taskkill /f /im dotnet.exe >nul 2>nul"

:: Go back to project root
cd /d "%PROJECT_DIR%"

echo.
echo ===== Setup Complete! =====
echo To run the API, navigate to the API project directory and use: dotnet run
echo   cd "%CD%\src\CodeFire.API"
echo   dotnet run
echo This will start the API at https://localhost:5001 and http://localhost:5000
echo.
echo To run tests, use: dotnet test
echo   cd "%CD%"
echo   dotnet test
echo.

:: Pause to let the user read the output
pause 