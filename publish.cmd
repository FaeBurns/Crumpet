git pull
dotnet publish Crumpet.Console -r win-x64 -c Release -o output /p:IncludeAllContentForSelfExtract=true /p:SelfContained=true /p:PublishSingleFile=true
@echo off
del output\*.pdb

:: would not work in upper folder :c
cd output
del Crumpet.exe
rename "Crumpet.Console.exe" "Crumpet.exe"
cd ..

robocopy Crumpet.Tests\Examples\Interpreter\ output\examples\ > nul

@echo on