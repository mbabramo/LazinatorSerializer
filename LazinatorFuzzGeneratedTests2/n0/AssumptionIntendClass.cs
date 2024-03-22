using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public abstract partial class AssumptionIntendClass : PsychologistOriginallyClass, IAssumptionIntendClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (AssumptionIntendClass) obj;
            return (other.ChristianImpossible,other.SlowCop,other.ConfirmOperation,other.AssociateDistinguish,other.LiteraryWeek,other.SkyPersonal,other.TicketImage,other.InteractionPassion,other.SufferYear).Equals((ChristianImpossible,SlowCop,ConfirmOperation,AssociateDistinguish,LiteraryWeek,SkyPersonal,TicketImage,InteractionPassion,SufferYear));
        }

        public override int GetHashCode()
        {
            return (ChristianImpossible,SlowCop,ConfirmOperation,AssociateDistinguish,LiteraryWeek,SkyPersonal,TicketImage,InteractionPassion,SufferYear).GetHashCode();
        }

    }
}
