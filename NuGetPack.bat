@echo off
set /p version="Version: "
msbuild SignalR.HubStrong.sln /P:Configuration=Release
rmdir /S /Q nuget-pack\lib
xcopy SignalR.HubStrong\bin\Release\SignalR.HubStrong.dll nuget-pack\lib\4.5\ /Y
.nuget\nuget pack nuget-pack\SignalR.HubStrong.nuspec -Version %version%
pause