namespace Library.BuildingBlocks.Infrastructure;

public interface IDomainNotificationsRegistry
{
    string GetName(Type type);

    Type GetType(string name);
}