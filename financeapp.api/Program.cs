using FinanceApp.Data;
using FinanceApp.Services;
using FinanceApp.Services.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IQuoteTaskRepository, QuoteTaskRepository>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IQuoteValidationService, QuoteValidationService>();
builder.Services.AddScoped<ICustomerValidationsService, CustomerValidationService>();
builder.Services.AddScoped<IJobValidationService, JobValidationService>();
builder.Services.AddScoped<IQuoteService, QuoteService>();

builder.Services.AddDbContext<FinanceAppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("FinanceDb"), 
        x => x.MigrationsAssembly("FinanceApp.Data"));
    //options.LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:8080")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("DevelopmentPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers().WithOpenApi();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            logger.LogError(exceptionHandlerPathFeature.Error, "An unhandled exception occurred.");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
    });
});

QuestPDF.Settings.License = LicenseType.Community;

DbInitializer.Seed(app.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceAppDbContext>());

app.Run();

