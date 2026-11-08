
using System.Collections.Generic;

namespace RobotPlanet.Core
{
    public class WorldDto
    {
        // Kõik maailma robotid
        public List<RobotDto> Robots { get; set; } = new();
    }
}
