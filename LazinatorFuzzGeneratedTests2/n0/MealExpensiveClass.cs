using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public partial class MealExpensiveClass : RefugeeSmartClass, IMealExpensiveClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (MealExpensiveClass) obj;
            return (other.SharpSafety,other.PopularExtremely,other.ConnectionAttorney,other.TicketImage,other.InteractionPassion,other.SufferYear).Equals((SharpSafety,PopularExtremely,ConnectionAttorney,TicketImage,InteractionPassion,SufferYear));
        }

        public override int GetHashCode()
        {
            return (SharpSafety,PopularExtremely,ConnectionAttorney,TicketImage,InteractionPassion,SufferYear).GetHashCode();
        }

    }
}
