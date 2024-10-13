@echo off
setlocal enabledelayedexpansion

:: Get the current directory (assumes the .bat file is in the project folder)
set "PROJECT_PATH=%CD%"

:: Path to the ProjectVersion.txt file
set "VERSION_FILE=%PROJECT_PATH%\ProjectSettings\ProjectVersion.txt"

:: Check if the version file exists
if not exist "%VERSION_FILE%" (
    echo Error: ProjectVersion.txt not found. Is this a Unity project folder?
    pause
    exit /b 1
)

:: Extract the Unity version from the file
for /f "tokens=2 delims=: " %%a in ('type "%VERSION_FILE%" ^| findstr /C:"m_EditorVersion"') do (
    set "UNITY_VERSION=%%a"
)

:: Remove any potential quotes from the version string
set UNITY_VERSION=%UNITY_VERSION:"=%

:: Construct the path to Unity executable
set "UNITY_PATH=C:\Program Files\Unity\Hub\Editor\%UNITY_VERSION%\Editor\Unity.exe"

:: Check if the Unity executable exists
if not exist "%UNITY_PATH%" (
    echo Error: Unity %UNITY_VERSION% not found at %UNITY_PATH%
    echo Please make sure the correct version is installed.
    pause
    exit /b 1
)

:: Launch Unity with the project path
start "" "%UNITY_PATH%" -projectPath "%PROJECT_PATH%"

echo Launching Unity %UNITY_VERSION% with the current project...
exit /b 0