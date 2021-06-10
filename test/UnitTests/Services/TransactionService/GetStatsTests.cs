using System;
using System.Collections.Generic;
using System.Globalization;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Constants;
using Core.Entities;
using Moq;
using Repository.Contracts;
using Xunit;

namespace UnitTests
{
    public class GetStatsTests
    {
        private readonly Fixture _fixture;

        public GetStatsTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        }
        
        [Fact]
        public void GetStats_Should_Return_Default_Values_When_No_Transactions_Exist()
        {
            var now = DateTime.UtcNow;
            
            var transactionRepository = _fixture.Freeze<Mock<ITransactionRepository>>();
            transactionRepository.Setup(m => m.GetTransactionsSince(It.IsAny<DateTime>())).Returns(new List<TransactionEntity>());

            var transactionService = _fixture.Create<Service.Implementations.TransactionService>();

            var stats = transactionService.GetStats(now);
            
            Assert.Equal("0.0", stats.Sum);
            Assert.Equal("0.0", stats.Avg);
            Assert.Equal("0.0", stats.Max);
            Assert.Equal("0.0", stats.Min);
            Assert.Equal(0, stats.Count);
            
            transactionRepository.Verify(m => m.GetTransactionsSince(It.Is<DateTime>(p => p == now.AddSeconds(-1 * ServiceConstants.OffsetInSeconds))), Times.Once);
        }
        
        [Fact]
        public void GetStats_Should_Return_Stats_When_Transactions_Exist()
        {
            var now = DateTime.UtcNow;

            var firstTransaction = new TransactionEntity
            {
                Amount = 10.0m,
                Timestamp = now.AddSeconds(-30)
            };
            
            var secondTransaction = new TransactionEntity
            {
                Amount = 20.0m,
                Timestamp = now.AddSeconds(-20)
            };

            var transactions = new List<TransactionEntity> {firstTransaction, secondTransaction};
            
            var transactionRepository = _fixture.Freeze<Mock<ITransactionRepository>>();
            transactionRepository.Setup(m => m.GetTransactionsSince(It.IsAny<DateTime>())).Returns(transactions);

            var transactionService = _fixture.Create<Service.Implementations.TransactionService>();

            var stats = transactionService.GetStats(now);
            
            Assert.Equal((firstTransaction.Amount + secondTransaction.Amount).ToString(CultureInfo.InvariantCulture), stats.Sum);
            Assert.Equal(((firstTransaction.Amount + secondTransaction.Amount) / 2).ToString(CultureInfo.InvariantCulture), stats.Avg);
            Assert.Equal(Math.Max(firstTransaction.Amount, secondTransaction.Amount).ToString(CultureInfo.InvariantCulture), stats.Max);
            Assert.Equal(Math.Min(firstTransaction.Amount, secondTransaction.Amount).ToString(CultureInfo.InvariantCulture), stats.Min);
            Assert.Equal(transactions.Count, stats.Count);
            
            transactionRepository.Verify(m => m.GetTransactionsSince(It.Is<DateTime>(p => p == now.AddSeconds(-1 * ServiceConstants.OffsetInSeconds))), Times.Once);
        }
    }
}