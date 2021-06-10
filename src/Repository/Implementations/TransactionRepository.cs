using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Repository.Contracts;

namespace Repository.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ConcurrentBag<TransactionEntity> _transactions = new();
        
        public void AddTransaction(TransactionEntity transactionModel)
        {
            _transactions.Add(transactionModel);
        }

        public void DeleteTransactions()
        {
            _transactions.Clear();
        }

        public List<TransactionEntity> GetTransactionsSince(DateTime since)
        {
            return _transactions.Where(t => since < t.Timestamp).ToList();
        }
    }
}