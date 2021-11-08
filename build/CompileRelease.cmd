SET msbuildpath=C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
SET sourcedir=..\src\UntappdViewer.sln
SET releasedir=..\src\UntappdViewer\bin\Release

"%msbuildpath%"\MSBuild.exe "%sourcedir%" /property:Configuration=Release
Xcopy /E /I "%releasedir%" UntappdViewer