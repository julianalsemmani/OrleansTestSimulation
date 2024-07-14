
namespace OrleansTestSimulation.Grains
{
    public class NodeGrain : Grain, INodeGrain
    {
        private readonly Guid id;

        public NodeGrain()
        {
            this.id = this.GetPrimaryKey();
        }
        public async Task<string> Ping()
        {
            return await Task.FromResult($"Pong from {id}");
        }
    }
}
