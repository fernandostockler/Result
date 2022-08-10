using ResultDemoWebApi.Services;

namespace ResultDemoWebApi.Controllers;

public static class WeatherForecastEndpoints
{
    public static void MapWeatherForecastEndpoints(this IEndpointRouteBuilder routes)
    {
        _ = routes.MapGet("/api/WeatherForecast",

            handler: async (WeatherForecastService weatherForecastService)
            => await weatherForecastService.Get())

            .WithTags(nameof(WeatherForecast))
            .WithName("GetAllWeatherForecasts")
            .Produces<List<WeatherForecast>>(StatusCodes.Status200OK);


        _ = routes.MapGet("/api/WeatherForecast/{id}",

            handler: async (int Id, WeatherForecastService weatherForecastService)
            => await weatherForecastService.GetById(Id))

            .WithTags(nameof(WeatherForecast))
            .WithName("GetWeatherForecastById")
            .Produces<WeatherForecast>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        _ = routes.MapPut("/api/WeatherForecast/{id}",

            handler: async (int Id, WeatherForecastService weatherForecastService)
            => await weatherForecastService.Update(Id))

            .WithTags(nameof(WeatherForecast))
            .WithName("UpdateWeatherForecast")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent);


        _ = routes.MapPost("/api/WeatherForecast/",

            handler: async (WeatherForecast weatherForecast, WeatherForecastService weatherForecastService)
            => await weatherForecastService.Add(weatherForecast))

            .WithTags(nameof(WeatherForecast))
            .WithName("CreateWeatherForecast")
            .Produces<WeatherForecast>(StatusCodes.Status201Created);


        _ = routes.MapDelete("/api/WeatherForecast/{id}",

            handler: async (int Id, WeatherForecastService weatherForecastService)
            => await weatherForecastService.Delete(Id))

            .WithTags(nameof(WeatherForecast))
            .WithName("DeleteWeatherForecast")
            .Produces<WeatherForecast>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
