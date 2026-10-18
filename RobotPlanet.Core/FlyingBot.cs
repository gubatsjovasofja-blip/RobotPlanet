
namespace RobotPlanet.Core
{
    public class FlyingBot : Robot
    {
        public FlyingBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " lendab planeedi kohal.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);
            return Name + " lendab tagurpidi ja teeb taevas saltosid!";
        }
    }
}
