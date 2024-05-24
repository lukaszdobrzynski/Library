namespace Library.BuildingBlocks.Domain;

public abstract class AggregateRootBase : Entity, IAggregateRoot
{
    public int VersionId { get; private set; }

    public void IncreaseVersion()
    {
        VersionId++;
    }
}