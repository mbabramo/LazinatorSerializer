using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public partial struct ChildPresidentStruct : IChildPresidentStruct
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (ChildPresidentStruct) obj;
            return (other.FlowStranger, other.VisitorWooden, other.LegendMan, other.StillOrdinary, other.TypeCareful, other.PrivateMusic).Equals((FlowStranger, VisitorWooden, LegendMan, StillOrdinary, TypeCareful, PrivateMusic));
        }

        public override int GetHashCode()
        {
            return (FlowStranger, VisitorWooden, LegendMan, StillOrdinary, TypeCareful, PrivateMusic).GetHashCode();
        }
    }
}
