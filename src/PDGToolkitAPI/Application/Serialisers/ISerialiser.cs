using System;

namespace PDGToolkitAPI.Application.Serialisers
{
    public interface ISerialiser
    {
        string Serialise<T>(T input);
    }
}