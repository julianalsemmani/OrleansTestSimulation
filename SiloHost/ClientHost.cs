using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class ClientHost
{
    public static async Task<IClusterClient> ConnectClient()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

        var client = new ClientBuilder()
            .UseLocalhostClustering(gatewayOptions =>
            {
                gatewayOptions.Gateways = new List<Uri>
                {
                    new Uri("http://localhost:30000"),
                    new Uri("http://localhost:30001"),
                    new Uri("http://localhost:30002")
                };
            })
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "NodeSimulation";
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton(serviceCollection);
            })
            .ConfigureLogging(logging => logging.AddConsole())
            .Build();

        await client.Connect();
        return client;
    }
}
