SET msbuildpath=E:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin
SET slndir=..\src\UntappdViewer.sln
SET releasedir=..\src\UntappdViewer\bin\Release
SET targetosversion=net8.0-windows10.0.22621.0
SET targetdir=UntappdViewer
SET logfile=MSBuild.Log.txt

Del %logfile%
Rmdir /S /Q %targetdir%
Rmdir /S /Q %releasedir%
"%msbuildpath%"\MSBuild.exe "%slndir%" /property:Configuration=Release > %logfile%
Xcopy /E /I "%releasedir%"\"%targetosversion%" %targetdir%
pause