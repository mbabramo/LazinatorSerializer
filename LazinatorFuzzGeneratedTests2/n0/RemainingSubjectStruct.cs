using System.Diagnostics.CodeAnalysis;
namespace FuzzTests.n0
{
    public partial struct RemainingSubjectStruct : IRemainingSubjectStruct
    {

       public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            var other = (RemainingSubjectStruct) obj;
            return (other.ThemeBefore,other.SongCrucial,other.RecentlySingle).Equals((ThemeBefore,SongCrucial,RecentlySingle));
        }

        public override int GetHashCode()
        {
            return (ThemeBefore,SongCrucial,RecentlySingle).GetHashCode();
        }

    }
}
