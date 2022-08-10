using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResultDemoWebApi;

namespace ResultDemoWebApi.Data
{
    public class ResultDemoWebApiContext : DbContext
    {
        public ResultDemoWebApiContext (DbContextOptions<ResultDemoWebApiContext> options)
            : base(options)
        {
        }

        public DbSet<ResultDemoWebApi.WeatherForecast> WeatherForecast { get; set; } = default!;
    }
}
