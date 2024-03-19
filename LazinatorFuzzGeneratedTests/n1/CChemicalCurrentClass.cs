using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public partial class CChemicalCurrentClass : IndependenceEmptyClass, ICChemicalCurrentClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (CChemicalCurrentClass) obj;
            return (other.ContentTalent).Equals((ContentTalent));
        }

        public override int GetHashCode()
        {
            return (ContentTalent).GetHashCode();
        }
    }
}
