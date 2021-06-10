using System;
using System.Globalization;
using System.Linq;
using Core.Constants;
using Core.Models;
using Repository.Contracts;
using Service.Contracts;

namespace Service.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IValidatorService _validatorService;

        public TransactionService(ITransactionRepository transactionRepository, IValidatorService validatorService)
        {
            _transactionRepository = transactionRepository;
            _validatorService = validatorService;
        }

        public void CreateTransaction(TransactionModel transactionModel, DateTime now)
        {
            var transactionEntity = _validatorService.ValidateTransaction(transactionModel, now);

            _transactionRepository.AddTransaction(transactionEntity);
        }

        public void DeleteTransactions()
        {
            _transactionRepository.DeleteTransactions();
        }

        public StatsModel GetStats(DateTime now)
        {
            var transactions = _transactionRepository.GetTransactionsSince(now.AddSeconds(-1 * ServiceConstants.TransactionTimeOffsetInSeconds));
            if (!transactions.Any())
            {
                return new StatsModel();
            }
            
            return new StatsModel
            {
                Sum = transactions.Sum(t => t.Amount).ToString("F", CultureInfo.InvariantCulture),
                Avg = transactions.Average(t => t.Amount).ToString("F", CultureInfo.InvariantCulture),
                Max = transactions.Max(t => t.Amount).ToString("F", CultureInfo.InvariantCulture),
                Min = transactions.Min(t => t.Amount).ToString("F", CultureInfo.InvariantCulture),
                Count = transactions.Count
            };
        }
    }
}