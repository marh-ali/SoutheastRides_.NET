using MongoDB.Driver;
using SoutheastRides.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = builder.Configuration.GetSection("Settings")
    .Get<SoutheastRidesDatabaseSettings>();

builder.Services.AddSingleton<ISoutheastRidesDatabaseSettings>(settings);

var client = new MongoClient(settings.ConnectionString);
var database = client.GetDatabase(settings.DatabaseName);

builder.Services.AddSingleton<IMongoDatabase>(database);
builder.Services.AddScoped(sp =>
    new SoutheastRidesContext(new MongoClient(settings.ConnectionString), settings.DatabaseName));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRideRepository, RideRepository>();
builder.Services.AddScoped<IRideService, RideService>();

builder.Services.AddScoped<IRsvpRepository, RsvpRepository>();
builder.Services.AddScoped<IRsvpService, RsvpService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
