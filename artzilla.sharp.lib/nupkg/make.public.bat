nuget.exe pack ..\nuget-package\ArtZilla.Sharp.Lib.nuspec -symbols
for %%f in (*.nupkg) do copy %%f N:\