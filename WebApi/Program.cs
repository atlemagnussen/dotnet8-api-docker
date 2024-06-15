using Microsoft.AspNetCore.OData;
using TestNet8.WebApi.Services;
//using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
//using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TestNet8.WebApi.Metrix;
using OpenTelemetry.Exporter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddOpenTelemetry(otel => {
    otel.IncludeFormattedMessage = true;
    otel.IncludeScopes = true;
});

string appName = "TestNet8Api";
if (builder.Environment.IsDevelopment())
    appName = $"{appName}-Debug";

builder.Services.AddOpenTelemetry()
    .ConfigureResource(x => {
        x.AddService(serviceName: appName);
    })
    .WithMetrics(x => {
        x.AddRuntimeInstrumentation()
            .AddMeter("Microsoft.AspNetCore.Hosting", 
                    "Microsoft.AspNetCore.Server.Kestrel",
                    "System.Net.Http",
                    "TestNet8.Controllers");
    })
    .WithTracing(x => {
        if (builder.Environment.IsDevelopment()) {
            x.SetSampler<AlwaysOnSampler>();
        }
        x.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    });

Uri? defaultOtlpEndpoint = null;
OtlpExportProtocol? defaultOtlpProtocol = null;
string? headers = null;

builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter(otlp => {
    defaultOtlpEndpoint = otlp.Endpoint;
    defaultOtlpProtocol = otlp.Protocol;
    headers = otlp.Headers; 
}));
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

var logger = app.Services.GetService<ILogger<Program>>();

logger.LogInformation($"default Otlp Endpoint={defaultOtlpEndpoint}, Protocol={defaultOtlpProtocol}, headers={headers}");

var otlpExporter = builder.Configuration.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT");
if (otlpExporter is not null)
    logger.LogInformation($"OTEL_EXPORTER_OTLP_ENDPOINT={otlpExporter}");
else
    logger.LogWarning("OTEL_EXPORTER_OTLP_ENDPOINT was null");

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
