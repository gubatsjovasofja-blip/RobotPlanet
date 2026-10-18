
namespace RobotPlanet.Core
{
    public class ChefBot : Robot
    {
        public ChefBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " valmistab maitsvat robotitoitu.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);
            return Name + " valmistab hiiglasliku pitsa kogu planeedile!";
        }
    }
}




