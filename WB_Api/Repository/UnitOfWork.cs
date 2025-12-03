using WB_Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using WB_Api.IRepository;
using System.Threading.Tasks;

namespace WB_Api.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<scanner>? _scanner;


        //public IGenericRepository<scanner> Scanner;

        IGenericRepository<scanner> IUnitOfWork.Scanner => throw new NotImplementedException();

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

       
        public Action IGenericRepository<scanner>()
        {

            return () =>
            {
                if (_scanner != null)
                {
                    _scanner = (IGenericRepository<Data.scanner>)_context;
                }
            };
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
