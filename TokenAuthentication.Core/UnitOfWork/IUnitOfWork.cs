using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TokenAuthentication.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
    }
}
