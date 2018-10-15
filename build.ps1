gci -recurse *.vcxproj | % { msbuild /m $_.FullName }
gci -recurse *.csproj | % { dotnet restore $_.FullName; dotnet build $_.FullName }
