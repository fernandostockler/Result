namespace ResultDemoWebApi.Services;

using Microsoft.EntityFrameworkCore;
using ResultDemoWebApi.Data;

public class WeatherForecastService
{
    private readonly ResultDemoWebApiContext db;

    public WeatherForecastService(ResultDemoWebApiContext db)
    {
        this.db = db;
    }

    public async Task<IResult> Get() => Results.Ok(await db.WeatherForecast.ToListAsync());

    public async Task<IResult> GetById(int Id)
    {
        return await db.WeatherForecast.FindAsync(Id)
            is WeatherForecast model
                ? Results.Ok(model)
                : Results.NotFound();
    }

    public async Task<IResult> Update(int Id)
    {
        WeatherForecast? foundModel = await db.WeatherForecast.FindAsync(Id);

        if (foundModel is null) return Results.NotFound();

        //update model properties here

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public async Task<IResult> Add(WeatherForecast weatherForecast)
    {
        db.WeatherForecast.Add(weatherForecast);
        await db.SaveChangesAsync();
        return Results.Created($"/WeatherForecasts/{weatherForecast.Id}", weatherForecast);
    }

    public async Task<IResult> Delete(int Id)
    {
        if (await db.WeatherForecast.FindAsync(Id) is WeatherForecast weatherForecast)
        {
            db.WeatherForecast.Remove(weatherForecast);
            await db.SaveChangesAsync();
            return Results.Ok(weatherForecast);
        }

        return Results.NotFound();
    }
}