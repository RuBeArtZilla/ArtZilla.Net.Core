nuget.exe pack ArtZilla.Sharp.Lib.nuspec
for %%f in (*.nupkg) do copy %%f C:\Web\Nuget\www\Packages