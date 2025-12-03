using WB_Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WB_Api.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<scanner> Scanner { get; }

        Task Save();
    }
}
