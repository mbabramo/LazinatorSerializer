using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public sealed partial class TimeShellClass :  ITimeShellClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (TimeShellClass) obj;
            return (other.HappyRecall, other.TalentSometimes, other.PaleCross).Equals((HappyRecall, TalentSometimes, PaleCross));
        }

        public override int GetHashCode()
        {
            return (HappyRecall, TalentSometimes, PaleCross).GetHashCode();
        }
    }
}
