using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public partial struct CommanderPersuadeStruct : ICommanderPersuadeStruct
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (CommanderPersuadeStruct) obj;
            return (other.PlaceRecently, other.ReasonableCan, other.MealExpensive, other.SingerSharp, other.LotsPopular, other.CustomerConnection, other.PartnerTrip, other.ThingPressure, other.OpinionVictim).Equals((PlaceRecently, ReasonableCan, MealExpensive, SingerSharp, LotsPopular, CustomerConnection, PartnerTrip, ThingPressure, OpinionVictim));
        }

        public override int GetHashCode()
        {
            return (PlaceRecently, ReasonableCan, MealExpensive, SingerSharp, LotsPopular, CustomerConnection, PartnerTrip, ThingPressure, OpinionVictim).GetHashCode();
        }
    }
}
