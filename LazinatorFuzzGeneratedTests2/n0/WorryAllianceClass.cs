using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public sealed partial class WorryAllianceClass :  IWorryAllianceClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (WorryAllianceClass) obj;
            return (other.MentalBasketball,other.WoodPersonality,other.FavoriteVisible).Equals((MentalBasketball,WoodPersonality,FavoriteVisible));
        }

        public override int GetHashCode()
        {
            return (MentalBasketball,WoodPersonality,FavoriteVisible).GetHashCode();
        }

    }
}
