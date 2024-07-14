namespace OrleansTestSimulation.Grains
{
    public interface INodeGrain
    {
        Task<string> Ping();
    }
}
