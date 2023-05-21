# Weather API

This is an API that wraps The Weather Channel's `api.weather.com`. It has three basic functions: `current`, `forecast` and `history`.

## How to build

The program should be run/built through Visual Studio. The program requires that a secret is added to the project. To do so, run this command while in the project directory:
```
dotnet user-secrets init
dotnet user-secrets set "ApiKey" "{apikey}"
```

Replace `{apikey}` with your api key.

## Available Routes
- `/weather/{city}` gets the current weather in the requested city.
- `/weather/{city}/forecast` gets a 5 day forecast for the requested city.
- `/weather/{city}/history/{numberOfDays}` gets weather up to 30 days in the past for the requested city. `{numberOfDays}` is optional and defaults to 30 if not set.

Replace `{city}` with your requested city and `{numberOfDays}` with how many days to display.
