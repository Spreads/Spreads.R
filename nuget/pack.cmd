dotnet restore ..\R.NET
dotnet pack ..\R.NET -c RELEASE -o ..\artifacts --version-suffix alpha

pause