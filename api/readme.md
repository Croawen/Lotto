Api project is hosted on http://hackyeah.azurewebsites.net

Requirements to build the api project:
1. .NET Core SDK (v2.1)
2. Visual Studio 2017 OR Visual Studio Code
3. MS SQL SERVER (Preferably 2017 edition) OR a connection string to the ms sql database hosted somewhere else.

Instructions for Visual Studio 2017:
1. Open Lotto.sln by clicking on File -> Open -> Project/Solution option in Visual Studio 2017.
2. Open appsettings.Development.json in /api/Lotto/Lotto.Api and replace default connection string with your own.
3. Run the solution - all packages will be restored automatically.

Instructions for Visual Studio Code
1. Open appsettings.Development.json in /api/Lotto/Lotto.Api and replace default connection string with your own.
2. Open terminal in /api/Lotto/Lotto.Api folder.
3. Run 'dotnet restore' command in the terminal to restore required packages.
4. Run 'dotnet run' command in the terminal to start the app. 

There is a test user created in the database.
Email: asdf@asdf.pl
Password: asdf