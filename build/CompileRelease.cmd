SET msbuildpath=C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
SET slndir=..\src\UntappdViewer.sln
SET releasedir=..\src\UntappdViewer\bin\Release
SET targetdir=UntappdViewer

Rmdir /S /Q %targetdir%
Rmdir /S /Q %releasedir%
"%msbuildpath%"\MSBuild.exe "%slndir%" /property:Configuration=Release
Xcopy /E /I "%releasedir%" %targetdir%