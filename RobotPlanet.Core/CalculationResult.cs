
namespace RobotPlanet.Core
{
    public class CalculationResult
    {
        // Roboti nimi
        public string RobotName { get; set; } = "";

        // Roboti tüüp
        public string RobotType { get; set; } = "";

        // Arvutuse tulemus
        public double Result { get; set; }

        // Staatus: valmis, viga või tühistatud
        public string Status { get; set; } = "";

        // Veateade
        public string Error { get; set; } = "";

        // Arvutuse aeg millisekundites
        public long TimeMs { get; set; }
    }
}
