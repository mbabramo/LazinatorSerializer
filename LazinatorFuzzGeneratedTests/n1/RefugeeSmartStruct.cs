using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public partial struct RefugeeSmartStruct : IRefugeeSmartStruct
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (RefugeeSmartStruct) obj;
            return (other.TicketImage, other.InteractionPassion).Equals((TicketImage, InteractionPassion));
        }

        public override int GetHashCode()
        {
            return (TicketImage, InteractionPassion).GetHashCode();
        }
    }
}
