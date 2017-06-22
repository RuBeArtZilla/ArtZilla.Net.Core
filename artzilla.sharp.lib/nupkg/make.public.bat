nuget.exe pack ..\nuget-package\ArtZilla.Net.Core.nuspec -symbols
for %%f in (*.nupkg) do copy %%f C:\Web\Nuget\www\Packages