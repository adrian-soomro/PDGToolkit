namespace PDGToolkitCore.API.Serialisers
{
    public interface ISerialiser
    {
        string Serialise<T>(T input);
    }
}