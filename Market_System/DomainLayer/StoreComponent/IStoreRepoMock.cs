using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.StoreComponent
{
    public interface IStoreRepoMock // This class is for unit tests purposes only.
    {
        void Save(Product toSave);
    }
}
