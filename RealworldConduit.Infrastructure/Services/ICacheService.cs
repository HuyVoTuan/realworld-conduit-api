using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealworldConduit.Infrastructure.Services
{
    public interface ICacheService
    {
        T GetItem<T>(string key);
        bool SetItem<T>(string key, T value, TimeSpan expirationTime);
        object RemoveItem(string key);
    }
}
