
**Start Docker Compose For Local Development And For Running Integration Test Locally**

docker-compose -p order-service-dev up --build --detach
docker-compose -p order-service-dev down --volumes 

Commands EF:

dotnet ef migrations add InitialOrderService --project src\OrderService.Infrastructure  --startup-project src\OrderService.Host -c OrdersDbContext -o Migrations


ToDo:

	- Improve Domain layer and Domain UnitTest
	- Add Delete & Update?
	- Add Consumer
	- Add Continous Integration
	- Add Readme
	
	