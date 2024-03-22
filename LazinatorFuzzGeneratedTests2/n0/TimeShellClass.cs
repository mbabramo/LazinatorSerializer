using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public sealed partial class TimeShellClass : RefugeeSmartClass, ITimeShellClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (TimeShellClass) obj;
            return (other.HappyRecall,other.TicketImage,other.InteractionPassion,other.SufferYear).Equals((HappyRecall,TicketImage,InteractionPassion,SufferYear));
        }

        public override int GetHashCode()
        {
            return (HappyRecall,TicketImage,InteractionPassion,SufferYear).GetHashCode();
        }

    }
}
