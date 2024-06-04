namespace Library.BuildingBlocks.Infrastructure;

public class DomainNotificationsRegistry : IDomainNotificationsRegistry
{
    private readonly Dictionary<string, Type> _map;

    public DomainNotificationsRegistry(Dictionary<string, Type> nameToTypeMap)
    {
        _map = nameToTypeMap;
    }

    public string GetName(Type type) => _map.Single(x => x.Value == type).Key;

    public Type GetType(string name) => _map[name];
}