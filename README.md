# ApiNativeAot
This is a simple API that built using .NET 8 with Native AOT support.

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
 
