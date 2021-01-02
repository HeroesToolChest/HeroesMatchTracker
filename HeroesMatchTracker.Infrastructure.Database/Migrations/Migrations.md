## Adding Migrations

## Via Package Manager Console
Set default project to `HeroesMatchTracker.Infrastructure.Database`
```
Add-Migration <name> -OutputDir Migrations/HeroesReplays -Startup HeroesMatchTracker.Infrastructure.Database -Context HeroesReplaysDbContext
```

## Via CLI
Move into `HeroesMatchTracker.Infrastructure.Database` directory
```
dotnet ef migrations add <name> --context HeroesReplaysDbContext --output-dir Migrations/HeroesReplays
```
