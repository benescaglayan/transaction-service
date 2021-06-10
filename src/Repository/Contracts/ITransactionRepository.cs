using System;
using System.Collections.Generic;
using Core.Entities;

namespace Repository.Contracts
{
    public interface ITransactionRepository
    {
        void AddTransaction(TransactionEntity transactionModel);

        void DeleteTransactions();

        List<TransactionEntity> GetTransactionsSince(DateTime since);
    }
}