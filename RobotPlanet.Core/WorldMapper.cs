
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RobotPlanet.Core
{
    public static class WorldMapper
    {
        // Muudan robotite maailma salvestatavateks andmeteks
        public static WorldDto ToDto(RobotUniverse universe)
        {
            WorldDto world = new WorldDto();

            foreach (Robot robot in universe.GetAllRobots())
            {
                world.Robots.Add(new RobotDto
                {
                    Id = robot.Id,
                    Type = robot.RobotType,
                    Name = robot.Name,
                    Energy = robot.Energy,
                    GroupName = robot.GroupName
                });
            }

            return world;
        }

        // Taastan maailma salvestatud andmetest
        public static RobotUniverse FromDto(WorldDto world)
        {
            if (world == null || world.Robots == null)
                throw new InvalidDataException("Maailma andmed puuduvad.");

            RobotUniverse universe = new RobotUniverse();
            HashSet<Guid> ids = new HashSet<Guid>();

            foreach (RobotDto data in world.Robots)
            {
                if (data == null ||
                    data.Id == Guid.Empty ||
                    !ids.Add(data.Id) ||
                    string.IsNullOrWhiteSpace(data.Name) ||
                    string.IsNullOrWhiteSpace(data.GroupName))
                {
                    throw new InvalidDataException(
                        "Fail sisaldab vigaseid roboti andmeid.");
                }

                Robot robot;

                switch (data.Type)
                {
                    case "CleanerBot":
                        robot = new CleanerBot(data.Name);
                        break;
                    case "ExplorerBot":
                        robot = new ExplorerBot(data.Name);
                        break;
                    case "RepairBot":
                        robot = new RepairBot(data.Name);
                        break;
                    case "DanceBot":
                        robot = new DanceBot(data.Name);
                        break;
                    case "ChefBot":
                        robot = new ChefBot(data.Name);
                        break;
                    case "GuardBot":
                        robot = new GuardBot(data.Name);
                        break;
                    case "FlyingBot":
                        robot = new FlyingBot(data.Name);
                        break;
                    default:
                        throw new InvalidDataException(
                            "Tundmatu roboti tüüp: " + data.Type);
                }

                try
                {
                    robot.RestoreState(
                        data.Id,
                        data.Energy,
                        data.GroupName);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidDataException(
                        "Roboti andmed on vigased.", ex);
                }

                // Add loob uuesti roboti sündmuste tellimused
                universe.Add(robot);
            }

            return universe;
        }
    }
}
