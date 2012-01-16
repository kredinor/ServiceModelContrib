call "c:\Program Files (x86)\Microsoft Visual Studio 10.0\vc\vcvarsall.bat"
msbuild /m /p:Configuration=Release ServiceModelContrib.sln
.nuget\nuget.exe pack ServiceModelContrib\ServiceModelContrib.nuspec /Symbols /OutputDirectory Build\Release
.nuget\nuget.exe pack ServiceModelContrib.IoC.Unity\ServiceModelContrib.IoC.Unity.nuspec /Symbols /OutputDirectory Build\Release
echo Done