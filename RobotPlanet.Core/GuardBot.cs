
namespace RobotPlanet.Core
{
    public class GuardBot : Robot
    {
        public GuardBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " valvab robotite baasi.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);
            return Name + " kuulutab tantsiva varju ohtlikuks tulnukaks!";
        }
    }
}


