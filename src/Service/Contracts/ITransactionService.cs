using System;
using Core.Models;

namespace Service.Contracts
{
    public interface ITransactionService
    {
        void CreateTransaction(TransactionModel transactionModel, DateTime now);
        
        void DeleteTransactions();
        
        StatsModel GetStats(DateTime now);
    }
}