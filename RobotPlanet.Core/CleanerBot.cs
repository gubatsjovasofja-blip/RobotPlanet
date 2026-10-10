
namespace RobotPlanet.Core
{
    public class CleanerBot : Robot, IChargeable
    {
        public CleanerBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " koristab põrandat.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);
            return Name + " imes kogemata oma ratta sisse!";
        }


        public string Charge()
        {
            AddEnergy(20);
            return Name + " laadis akut. Energia: " + Energy;
        }

    }
}
