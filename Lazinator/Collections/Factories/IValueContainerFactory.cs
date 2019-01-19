namespace Lazinator.Collections.Factories
{
    public interface IValueContainerFactory<T>
    {
        IValueContainerFactory<T> InteriorFactory { get; set; }
        long InteriorContainerMax { get; set; }
    }
}