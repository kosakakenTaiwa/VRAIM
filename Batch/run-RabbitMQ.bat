@echo off
set DOCKER_NAME=Docker Desktop

:: Check if Docker Desktop is running
tasklist /FI "IMAGENAME eq Docker Desktop.exe" | find /I "%DOCKER_NAME%" > nul
if errorlevel 1 (
    echo %DOCKER_NAME% is not running. Starting %DOCKER_NAME%...
    start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    echo Waiting for %DOCKER_NAME% to start...
    :: Wait for Docker Desktop to start
    :WAIT
    timeout /T 5 /NOBREAK > nul
    tasklist /FI "IMAGENAME eq Docker Desktop.exe" | find /I "%DOCKER_NAME%" > nul
    if errorlevel 1 goto WAIT
)

echo %DOCKER_NAME% is running. Checking Docker daemon...

:: Wait for Docker daemon to be ready

echo Docker daemon is running.

:: Run the docker command
start cmd /k "docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management"
