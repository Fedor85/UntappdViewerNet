SET slndir=..\src\UntappdViewer.sln
SET nugetdir=..\tools\nuget

"%nugetdir%"\nuget.exe restore "%slndir%"

pause