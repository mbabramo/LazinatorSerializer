using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public abstract partial class CCrossTheaterClass : IndependenceEmptyClass, ICCrossTheaterClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (CCrossTheaterClass) obj;
            return (other.PersuadeSong, other.PlaceRecently).Equals((PersuadeSong, PlaceRecently));
        }

        public override int GetHashCode()
        {
            return (PersuadeSong, PlaceRecently).GetHashCode();
        }
    }
}
