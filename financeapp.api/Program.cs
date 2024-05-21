using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddDbContext<FinanceAppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FinanceDb"), x => x.MigrationsAssembly("FinanceApp.Data")));
builder.Services.AddControllers(); // Registers the necessary services for controllers

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Adjust the port number as necessary
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseCors("DevelopmentPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers().WithOpenApi();

DbInitializer.Seed(app.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceAppDbContext>());

app.Run();

