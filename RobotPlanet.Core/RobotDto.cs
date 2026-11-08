
using System;

namespace RobotPlanet.Core
{
    public class RobotDto
    {
        // Roboti andmed salvestamiseks
        public Guid Id { get; set; }
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
        public int Energy { get; set; }
        public string GroupName { get; set; } = "";
    }
}
