using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = builder.Configuration.GetSection("Settings")
    .Get<SouthwestRidesDatabaseSettings>();

builder.Services.AddSingleton<ISouthwestRidesDatabaseSettings>(settings);

var client = new MongoClient(settings.ConnectionString);
var database = client.GetDatabase(settings.DatabaseName);

builder.Services.AddSingleton<IMongoDatabase>(database);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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
