using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public partial struct TrueAttachStruct : ITrueAttachStruct
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (TrueAttachStruct) obj;
            return (other.BottomOpinion,other.PresidentMusic,other.FlowStranger,other.VisitorWooden).Equals((BottomOpinion,PresidentMusic,FlowStranger,VisitorWooden));
        }

        public override int GetHashCode()
        {
            return (BottomOpinion,PresidentMusic,FlowStranger,VisitorWooden).GetHashCode();
        }

    }
}
