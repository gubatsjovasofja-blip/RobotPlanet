
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RobotPlanet.Core
{
    public class JsonWorldStorage : IWorldStorage
    {
        // JSON-faili vormindamise seaded
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        // Salvestan maailma JSON-faili
        public async Task SaveAsync(
            WorldDto world,
            string path,
            CancellationToken token = default)
        {
            if (world == null)
                throw new ArgumentNullException(nameof(world));

            await using FileStream file = new FileStream(
                path, FileMode.Create, FileAccess.Write);

            await JsonSerializer.SerializeAsync(
                file, world, options, token);
        }

        // Laen maailma JSON-failist
        public async Task<WorldDto> LoadAsync(
            string path,
            CancellationToken token = default)
        {
            await using FileStream file = new FileStream(
                path, FileMode.Open, FileAccess.Read);

            WorldDto? world =
                await JsonSerializer.DeserializeAsync<WorldDto>(
                    file, options, token);

            if (world == null || world.Robots == null)
                throw new InvalidDataException(
                    "JSON-fail ei sisalda korrektset maailma.");

            return world;
        }
    }
}
