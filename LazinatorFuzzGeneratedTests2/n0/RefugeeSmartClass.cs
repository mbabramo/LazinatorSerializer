using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public partial class RefugeeSmartClass :  IRefugeeSmartClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (RefugeeSmartClass) obj;
            return (other.TicketImage,other.InteractionPassion,other.SufferYear).Equals((TicketImage,InteractionPassion,SufferYear));
        }

        public override int GetHashCode()
        {
            return (TicketImage,InteractionPassion,SufferYear).GetHashCode();
        }

    }
}
