﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_System.DomainLayer.StoreComponent
{
    public interface IStoreRepoMock
    {
        void Save(Product toSave);
    }
}
