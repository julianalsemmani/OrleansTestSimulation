using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;

namespace OrleansTestSimulation.SiloHost
{
    public class SiloHost
    {
        public static async Task<IList<IHost>> StartSilos(int totalSilos)
        {
            var silos = new List<IHost>();

            for (int i = 0; i < totalSilos; i++)
            {
                var silo = Host.CreateDefaultBuilder()
                    .UseOrleans(siloBuilder =>
                    {
                        siloBuilder.UseLocalhostClustering()
                            .Configure<ClusterOptions>(options =>
                            {
                                options.ClusterId = "dev";
                                options.ServiceId = "NodeSimulation";
                            })
                            .ConfigureEndpoints(siloPort: 11111 + i, gatewayPort: 30000 + i)
                            .ConfigureLogging(logging => logging.AddConsole());
                    })
                    .Build();

                await silo.StartAsync();
                silos.Add(silo);
            }

            return silos;
        }

        public static async Task StopSilos(List<IHost> silos)
        {
            foreach (var silo in silos)
            {
                await silo.StopAsync();
            }
        }
    }
}
