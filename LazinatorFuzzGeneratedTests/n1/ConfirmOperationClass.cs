using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public abstract partial class ConfirmOperationClass :  IConfirmOperationClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (ConfirmOperationClass) obj;
            return (other.DistinguishStart, other.WeekBar).Equals((DistinguishStart, WeekBar));
        }

        public override int GetHashCode()
        {
            return (DistinguishStart, WeekBar).GetHashCode();
        }
    }
}
