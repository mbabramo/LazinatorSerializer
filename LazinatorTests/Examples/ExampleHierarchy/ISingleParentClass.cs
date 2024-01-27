using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.ExampleHierarchy
{
    [Lazinator((int) ExampleUniqueIDs.SingleParentClass)]
    [SingleParent]
    public interface ISingleParentClass
    {
        LazinatorList<WInt16> SomeLazinator { get; set; }
    }
}