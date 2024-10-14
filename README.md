CatApi (based on TheCatApi.com)

+ Installation Prerequisites

  - TheCatApi.com Api Key
  - MS SQL Server


+ Method 1 (git clone)

  - Install dotnet 8 SDK
  - Install dotnet-ef tool
  - In the appsettings.json, replace CONSTR with your DB connection string and CATAPIKEY with your api key from TheCatApi.com.
      > $ dotnet-ef migrations add Initial
      
      > $ dotnet-ef database update
      
  - In CatApi/CatApi/ run
      > $ dotnet run


+ Method 2 (Dockerfile)

  - Build dockerfile 
      > $ docker build -t  catapi .
  - Run docker image
      > $ docker run -d -p 8080:8080 catapi
