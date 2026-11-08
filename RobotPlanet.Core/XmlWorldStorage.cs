
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RobotPlanet.Core
{
    public class XmlWorldStorage : IWorldStorage
    {
        // XML-serializer maailma andmete jaoks
        private readonly XmlSerializer serializer =
            new XmlSerializer(typeof(WorldDto));

        // Salvestan maailma XML-faili
        public async Task SaveAsync(
            WorldDto world,
            string path,
            CancellationToken token = default)
        {
            if (world == null)
                throw new ArgumentNullException(nameof(world));

            token.ThrowIfCancellationRequested();

            // Teen salvestamise eraldi ülesandes
            await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                using FileStream file = new FileStream(
                    path, FileMode.Create, FileAccess.Write);

                serializer.Serialize(file, world);
            }, token);
        }

        // Laen maailma XML-failist
        public async Task<WorldDto> LoadAsync(
            string path,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                using FileStream file = new FileStream(
                    path, FileMode.Open, FileAccess.Read);

                WorldDto? world =
                    serializer.Deserialize(file) as WorldDto;

                if (world == null || world.Robots == null)
                    throw new InvalidDataException(
                        "XML-fail ei sisalda korrektset maailma.");

                return world;
            }, token);
        }
    }
}
