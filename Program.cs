
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PatientProject;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build();

host.Run();
