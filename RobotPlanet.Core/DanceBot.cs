namespace RobotPlanet.Core
{
    public class DanceBot : Robot
    {
        public DanceBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " tantsib muusika saatel.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);
            return Name + " korraldab suure robotidisko!";
        }
    }
}