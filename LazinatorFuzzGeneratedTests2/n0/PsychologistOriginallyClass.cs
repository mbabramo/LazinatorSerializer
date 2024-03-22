using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public partial class PsychologistOriginallyClass : RefugeeSmartClass, IPsychologistOriginallyClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (PsychologistOriginallyClass) obj;
            return (other.ConfirmOperation,other.AssociateDistinguish,other.LiteraryWeek,other.SkyPersonal,other.TicketImage,other.InteractionPassion,other.SufferYear).Equals((ConfirmOperation,AssociateDistinguish,LiteraryWeek,SkyPersonal,TicketImage,InteractionPassion,SufferYear));
        }

        public override int GetHashCode()
        {
            return (ConfirmOperation,AssociateDistinguish,LiteraryWeek,SkyPersonal,TicketImage,InteractionPassion,SufferYear).GetHashCode();
        }

    }
}
