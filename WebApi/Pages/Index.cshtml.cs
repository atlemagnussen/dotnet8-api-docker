using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestNet8.WebApi.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string? IpAddress { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        IpAddress = GetIpAddress();
    }

    private string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return string.Empty;
    }
}