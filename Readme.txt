Commands EF:

dotnet ef migrations add InitialIdenityMigration --project src\OrderService.Infrastructure  --startup-project src\OrderService.Host -c OrdersDbContext -o Migrations