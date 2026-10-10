
namespace RobotPlanet.Core
{
    public class RepairBot : Robot, IChargeable, IScannable
    {
        public RepairBot(string name) : base(name)
        {
        }

        public override string NormalAction()
        {
            UseEnergy(10);
            return Name + " parandab katkist robotit.";
        }

        public override string CrazyAction()
        {
            UseEnergy(20);

            if (Energy < 40)
            {
                return Name + " on väsinud ja parandab kogemata külmkappi!";
            }

            return Name + " parandas roboti, aga unustas kruvid tagasi panna!";
        }

        public string Scan()
        {
            UseEnergy(5);
            return Name + " otsib katkiseid seadmeid.";
        }

        public string Charge()
        {
            AddEnergy(20);
            return Name + " laadis akut. Energia: " + Energy;
        }
    }
}
