using Microsoft.AspNetCore.OData;
using TestWebApiNet8Docker.Services;
//using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
//using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TestWebApiNet8Docker.Metrix;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddOpenTelemetry(otel => {
    otel.IncludeFormattedMessage = true;
    otel.IncludeScopes = true;
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(x => x.AddService(serviceName: "TestNet8Api"))
    .WithMetrics(x => {
        x.AddRuntimeInstrumentation()
            .AddMeter("Microsoft.AspNetCore.Hosting", 
                    "Microsoft.AspNetCore.Server.Kestrel",
                    "System.Net.Http",
                    "TestWebApiNet8Docker.Controllers");
    })
    .WithTracing(x => {
        if (builder.Environment.IsDevelopment()) {
            x.SetSampler<AlwaysOnSampler>();
        }
        x.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    });

builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

builder.Services.ConfigureHttpClientDefaults(http => {
    http.AddStandardResilienceHandler();
});

builder.Services.AddControllers().AddOData(options =>
{
    options.Filter();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMetrics();
builder.Services.AddSingleton<WeatherMetrics>();

builder.Services.AddTransient<ODataQueryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
