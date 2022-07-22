using TOTP.Models;
using TOTP.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddIniFile("config.ini");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS have been disabled for DEVELOPMENT environment
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins("*").AllowAnyHeader()
                                .AllowAnyMethod();
                        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/generateToken", (RequestToken req) =>
{
    String secret = builder.Configuration["secret"].ToString();
    TotpService totpService = new TotpService(secret);

    /*
     * Test request
     * {
     *    "username": "dani",
     *   "timestamp": 1657974234
     * }
     * 
     * test response
     * {
     *    "totp": "338422",
     *    "expiryTime": 1657974240
     *  }
     * 
     */

    ResponseToken result = new ResponseToken(
        ExpiryTime: totpService.GenerateExpiryTime(req.Timestamp),
        Totp: totpService.GenerateTotp(username: req.Username, unixtimestamp: req.Timestamp)
    );

    return result;
});

app.MapGet("/", () =>
{
    return;
});

if (app.Environment.IsDevelopment())
{
    app.UseCors(MyAllowSpecificOrigins);
}

app.Run();
