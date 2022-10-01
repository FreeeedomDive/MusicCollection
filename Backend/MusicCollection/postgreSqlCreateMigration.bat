@echo off
set /p "name=Enter migration name: "
dotnet tool install --global dotnet-ef
dotnet ef migrations add %name% --project MusicCollection.BusinessLogic --startup-project MusicCollection.Api
pause