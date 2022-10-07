set ASPNETCORE_ENVIRONMENT=Production
dotnet ef database update --project MusicCollection.BusinessLogic --startup-project MusicCollection.Api
pause