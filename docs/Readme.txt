
**Start Docker Compose For Local Development And For Running Integration Test Locally**

docker-compose -p order-service-dev up --build --detach

Commands EF:

dotnet ef migrations add InitialIdenityMigration --project src\OrderService.Infrastructure  --startup-project src\OrderService.Host -c OrdersDbContext -o Migrations