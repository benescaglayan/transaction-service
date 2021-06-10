using System;
using System.Globalization;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Service.Contracts;

namespace Service.Implementations
{
    public class ValidatorService : IValidatorService
    {
        public TransactionEntity ValidateTransaction(TransactionModel transactionModel, DateTime now)
        {
            if (!decimal.TryParse(transactionModel.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount) ||
                !DateTime.TryParseExact(transactionModel.Timestamp, ServiceConstants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var date) || 
                date > now || amount == 0)
            {
                throw new UnprocessableTransactionException();
            }

            if (date < now.AddSeconds(-1 * ServiceConstants.TransactionTimeOffsetInSeconds))
            {
                throw new LateReportedTransactionException();
            }
            
            var transactionEntity = new TransactionEntity
            {
                Amount = amount,
                Timestamp = date
            };

            return transactionEntity;
        }
    }
}