// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var fromDate = DateTime.Parse("2018-12-10T00:00:00Z");
Console.WriteLine($"Fromdate kind = {fromDate.Kind}, value={fromDate:u}");


fromDate = fromDate.ToUniversalTime();
Console.WriteLine($"Fromdate kind = {fromDate.Kind}, value={fromDate:u}");


fromDate = fromDate.ToUniversalTime();
Console.WriteLine($"Fromdate kind = {fromDate.Kind}, value={fromDate:u}");