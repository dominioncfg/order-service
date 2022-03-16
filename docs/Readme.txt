
* Start Docker Compose For Local Development And For Running Integration Test Locally:

** Start
docker-compose -p order-service-dev up --build --detach

** Stop
docker-compose -p order-service-dev down --volumes 

* Usefull dotnet Commands EF:

** Add new migration
dotnet ef migrations add InitialOrderService --project src\OrderService.Infrastructure  --startup-project src\OrderService.Host -c OrdersDbContext -o Migrations


	
