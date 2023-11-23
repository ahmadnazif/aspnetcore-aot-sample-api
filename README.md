# ApiNativeAot
This is a simple API server that built using .NET 8 with native AOT support. I've also built the similiar API server using:
- [NodeJS](https://github.com/ahmadnazif/nodejs-sample-api)
- [Quarkus (Java)](https://github.com/ahmadnazif/quarkus-sample-api)

## Key point
- API server with Ahead-of-time (AOT) compilation native support which improves app startup time
- Brand new .NET 8

## Data Storage
No external library is used. Instead, a singleton reference is created each time app started and hold the data temporarly in memory. You can see it in `Services/InMemorySimpleDb.cs`.

## Running sample using Visual Studio 2022
- Make sure you already installed .NET 8 LTS
- Clone this repo
- Restore all dependency `dotnet restore`
- Navigate to `ApiNativeAot` folder
  - In `appsettings.json`, set your preferred port. Default here is `5566`
  - Run `dotnet run`
- Next time, you can also directly run the app by the `.dll` file.
  - Navigate to `ApiNativeAot/bin/Debug/net8.0`
  - Run `dotnet ApiNativeAot.dll`

 ## Build & run using Docker
 - Clone this repo
 - Expose the same port in `appsettings.json` and `Dockerfile`
 - Build image: `docker build -f Dockerfile .. -t aspnetcore-aot-sample-api:v0.1` (This will create "aspnetcore-aot-sample-api" image with "v.01" tag)
 - Run container `docker run --name api-demo -p 5566:5566 -d aspnetcore-aot-sample-api:v0.1` (This will create & run "api-demo" container in background on port 5566)
