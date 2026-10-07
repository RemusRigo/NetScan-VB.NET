dotnet restore

@timeout 3

rem dotnet build -f net48 -c Release -p:Platform=x64
rem dotnet build -f net48 -c Release -p:Platform=x86

dotnet build -f net8.0-windows -c Release -p:Platform=x64
dotnet build -f net8.0-windows -c Release -p:Platform=x86

dotnet build -f net10.0-windows -c Release -p:Platform=x64
dotnet build -f net10.0-windows -c Release -p:Platform=x86

copy README.md .\bin\Release /Y

@timeout 7