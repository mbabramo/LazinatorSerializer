
namespace FuzzTests.n1
{
    public abstract partial class WorryAllianceClass :  IWorryAllianceClass
    {

       public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (WorryAllianceClass) obj;
            return (other.MentalBasketball,other.WoodPersonality).Equals((MentalBasketball,WoodPersonality));
        }

        public override int GetHashCode()
        {
            return (MentalBasketball,WoodPersonality).GetHashCode();
        }

    }
}
