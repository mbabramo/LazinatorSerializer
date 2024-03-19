using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n1
{
    public partial class IndependenceEmptyClass :  IIndependenceEmptyClass
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (IndependenceEmptyClass) obj;
            return (other.GalleryFavorite, other.BuryDrawing).Equals((GalleryFavorite, BuryDrawing));
        }

        public override int GetHashCode()
        {
            return (GalleryFavorite, BuryDrawing).GetHashCode();
        }
    }
}
