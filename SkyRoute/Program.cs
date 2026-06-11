using SkyRoute.Pricing;
using SkyRoute.Repository;
using SkyRoute.Services;
using SkyRoute.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<FlightService>();
builder.Services.AddScoped<IFareCalculator, FareCalculator>();
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS — allow Angular dev server
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();          // ← must be BEFORE UseAuthorization
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();