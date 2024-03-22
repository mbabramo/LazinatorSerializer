using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public abstract partial class RegularReasonableClass :  IRegularReasonableClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (RegularReasonableClass) obj;
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

    }
}
