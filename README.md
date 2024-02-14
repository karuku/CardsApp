Cards RESTful web service (C#/.NET, Sr)

Configurations:
1. To configure JWT(JSON Web Token) configuration settings, open the appsettings.json and find the section named "JWT" where you can change the values according to your requirements.
2. In the same file, the section "AppSettings" contains the serverType(SqlServer=1,PostGres=2, etc). NB: Kindly note that only SqlServer is supported
at this time, therefore kindly leave the "DbServerType" value as it is(1).
The "MigrateDb" setting allows for the application to automatically migrate the database when you run the application.
3. To configure "Database Connection" settings, open the connections.json and find the "ConnectionStrings" section and edit the connection string
under the "Database" setting.

The API endpoints are protected and you require authentication using the "authenticate" endpoint.
Default users are:
1. Username: admin@logicea.com (in Admin role)
2. Username: member@logicea.com (in Member role)

Password: User1234! 

Cheers!