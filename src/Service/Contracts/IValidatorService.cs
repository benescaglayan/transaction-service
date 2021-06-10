using System;
using Core.Entities;
using Core.Models;

namespace Service.Contracts
{
    public interface IValidatorService
    {
        TransactionEntity ValidateTransaction(TransactionModel transactionModel, DateTime now);
    }
}