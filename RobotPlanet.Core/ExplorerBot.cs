
namespace RobotPlanet.Core
{
    public class ExplorerBot : Robot, IScannable
    {
        public ExplorerBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " uurib uut planeeti.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);
            return Name + " leidis kivi ja arvas, et see on tulnukas!";
        }

        public string Scan()
        {
            UseEnergy(5);
            return Name + " skaneerib ümbrust.";
        }
    }
}
