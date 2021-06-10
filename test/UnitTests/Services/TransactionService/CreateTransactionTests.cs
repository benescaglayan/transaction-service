using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Models;
using Moq;
using Repository.Contracts;
using Service.Contracts;
using Xunit;

namespace UnitTests
{
    public class CreateTransactionTests
    {
        private readonly Fixture _fixture;

        public CreateTransactionTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        }
        
        [Fact]
        public void CreateTransaction_Should_Validate_Add_Transaction_To_TransactionRepository()
        {
            var now = DateTime.UtcNow;
            var transactionModel = new TransactionModel
            {
                Amount = "123.15",
                Timestamp = "2021-11-13T11:26:42.444Z"
            };
            
            var transactionEntity = new TransactionEntity
            {  
                Amount = 123.15m,
                Timestamp = new DateTime(2021, 11, 13, 11, 26, 42, 444, DateTimeKind.Utc)
            };
            
            var validatorService = _fixture.Freeze<Mock<IValidatorService>>();
            validatorService.Setup(m => m.ValidateTransaction(It.IsAny<TransactionModel>(), It.IsAny<DateTime>())).Returns(transactionEntity);
            
            var transactionRepository = _fixture.Freeze<Mock<ITransactionRepository>>();
            transactionRepository.Setup(m => m.AddTransaction(It.IsAny<TransactionEntity>())).Verifiable();

            var transactionService = _fixture.Create<Service.Implementations.TransactionService>();

            transactionService.CreateTransaction(transactionModel, now);
            
            validatorService.Verify(m => m.ValidateTransaction(It.Is<TransactionModel>(p => p.Amount == transactionModel.Amount && p.Timestamp == transactionModel.Timestamp), 
                It.Is<DateTime>(p => p == now)), Times.Once);
            transactionRepository.Verify(m => m.AddTransaction(It.Is<TransactionEntity>(p => p.Amount == transactionEntity.Amount && p.Timestamp == transactionEntity.Timestamp)), Times.Once);

        }
    }
}