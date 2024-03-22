using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public partial struct LotsEaseStruct : ILotsEaseStruct
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (LotsEaseStruct) obj;
            return (other.LargeStill,other.GroceryType).Equals((LargeStill,GroceryType));
        }

        public override int GetHashCode()
        {
            return (LargeStill,GroceryType).GetHashCode();
        }

    }
}
