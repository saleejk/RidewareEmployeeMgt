Instructions:
1- in appsettings.json, set the connection string according to your machine. and if needed, change the key for JWT Authentication.
2- I have seeded the initial data for the database in DbContextClass.cs. you may change it if needed.
Credentials for Initial user datas:-
(email: admin@gmail.com, password:12345678), (email: rideware@gmail.com, password:12345678)
3- I used Entity Framework for connection. so, complete a migration and update the database then you'll be ready to go. you can use this commands for migration "Add-Migration migration_name"

Points to note:-
I have 2 controllers. one is for registering a user and for login. and the other one is used to perform the CRUD on Employees Table.
incase of users. I have seeded 2 Initial users to the DB. those 2 Users will have admin access and that can be used to perform all operations.
the later registered users (through API) will only have limited access (Read only)
