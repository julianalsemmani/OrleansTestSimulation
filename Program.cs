using OrleansTestSimulation.Grains;
using OrleansTestSimulation.SiloHost;

public class Program
{
    public static async Task Main(string[] args)
    {
        var silos = await SiloHost.StartSilos(3); // Starter 3 siloer
        var client = await ClientHost.ConnectClient();

        // Simulerer flere noder
        int numberOfNodes = 10;
        var tasks = new List<Task>();

        for (int i = 0; i < numberOfNodes; i++)
        {
            int nodeId = i;
            tasks.Add(SimulateNode(client, nodeId));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine("Press any key to terminate...");
        Console.ReadKey();

        await client.Close();
        await SiloHost.StopSilos(silos);
    }

    private static async Task SimulateNode(IClusterClient client, int nodeId)
    {
        var grain = client.GetGrain<INodeGrain>(nodeId);
        var response = await grain.Ping();
        Console.WriteLine($"Response from node {nodeId}: {response}");
    }
}
