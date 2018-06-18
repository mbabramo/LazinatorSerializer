using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Abstract
{
    public partial class ConcreteFromGenericFromBase : GenericFromBase<WNullableDecimal>, IConcreteFromGenericFromBase
    {
    }
}
