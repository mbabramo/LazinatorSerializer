using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public sealed partial class WindAirlineClass :  IWindAirlineClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (WindAirlineClass) obj;
            return (other.SmallStop, other.LungTypically).Equals((SmallStop, LungTypically));
        }

        public override int GetHashCode()
        {
            return (SmallStop, LungTypically).GetHashCode();
        }
    }
}
