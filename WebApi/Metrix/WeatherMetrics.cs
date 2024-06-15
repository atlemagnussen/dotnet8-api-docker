using System.Diagnostics.Metrics;

namespace TestNet8.WebApi.Metrix;

public class WeatherMetrics
{
    public const string MeterName = "Weather.Api";
    public readonly Counter<long> _requestCounter;
    public readonly Histogram<double> _requestDuration;

    public WeatherMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _requestCounter = meter.CreateCounter<long>("api.request.count");
        _requestDuration = meter.CreateHistogram<double>("api.request.duration", "ms");
    }
    public void IncreaseRequestCount()
    {
        _requestCounter.Add(1);
    }

    public RequestDuration MeasureRequestDuration() {
        return new RequestDuration(_requestDuration);
    }
}

public class RequestDuration : IDisposable
{
    private readonly long _requestStartTime = TimeProvider.System.GetTimestamp();
    private readonly Histogram<double> _histogram;

    public RequestDuration(Histogram<double> histogram)
    {
        _histogram = histogram;
    }
    public void Dispose()
    {
        var elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime);
        _histogram.Record(elapsed.TotalMilliseconds);
    }
}