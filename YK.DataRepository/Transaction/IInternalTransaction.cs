using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace YK.DataRepository.Transaction
{
    internal interface IInternalTransaction : ITransaction
    {
        void BeginTransaction(IsolationLevel isolationLevel);
        Task BeginTransactionAsync(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();
        void DisposeTransaction();
    }
}
