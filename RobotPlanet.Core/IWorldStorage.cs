
using System.Threading;
using System.Threading.Tasks;

namespace RobotPlanet.Core
{
    public interface IWorldStorage
    {
        // Salvestan maailma andmed
        Task SaveAsync(
            WorldDto world,
            string path,
            CancellationToken token = default);

        // Laen maailma andmed
        Task<WorldDto> LoadAsync(
            string path,
            CancellationToken token = default);
    }
}

