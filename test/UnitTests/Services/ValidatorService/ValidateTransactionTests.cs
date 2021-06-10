using System;
using System.Globalization;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Constants;
using Core.Exceptions;
using Core.Models;
using Service.Implementations;
using Xunit;

namespace UnitTests
{
    public class ValidateTransactionTests
    {
        private readonly Fixture _fixture;

        public ValidateTransactionTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        }
        
        [Theory]
        [InlineData("abc")]
        [InlineData("0")]
        [InlineData("0.0")]
        [InlineData("123....14")]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData("145.5342.4")]
        public void ValidateTransaction_Should_Throw_UnprocessableTransactionException_When_Amount_Cannot_Be_Parsed_Or_Zero(string amount)
        {
            var now = DateTime.UtcNow;
            var transactionModel = new TransactionModel
            {
                Amount = amount,
                Timestamp = now.ToString(ServiceConstants.DateTimeFormat)
            };

            var transactionService = _fixture.Create<ValidatorService>();

            Assert.Throws<UnprocessableTransactionException>(() => transactionService.ValidateTransaction(transactionModel, now));
        }
        
        [Theory]
        [InlineData("abc")]
        [InlineData("     ")]
        [InlineData("")]
        [InlineData("2021-11-09")]
        [InlineData("2021/11/13T11:26:42.444Z")]
        [InlineData("2021-11-13T11:26:42Z")]
        [InlineData("3021-11-13T11:26:42.444Z")] // note: this test is era-dependent, it will eventually fail!
        public void ValidateTransaction_Should_Throw_UnprocessableTransactionException_When_Timestamp_Cannot_Be_Parsed_Or_Points_To_Future(string timestamp)
        {
            var now = DateTime.UtcNow;
            var transactionModel = new TransactionModel
            {
                Amount = "123.14",
                Timestamp = timestamp
            };

            var transactionService = _fixture.Create<ValidatorService>();

            Assert.Throws<UnprocessableTransactionException>(() => transactionService.ValidateTransaction(transactionModel, now));
        }
        
        [Fact]
        public void ValidateTransaction_Should_Throw_UnprocessableTransactionException_When_Timestamp_Points_Too_Past()
        {
            var now = DateTime.UtcNow;
            var transactionModel = new TransactionModel
            {
                Amount = "123.14",
                Timestamp = now.AddSeconds(-3 * ServiceConstants.TransactionTimeOffsetInSeconds).ToString(ServiceConstants.DateTimeFormat)
            };

            var transactionService = _fixture.Create<ValidatorService>();

            Assert.Throws<LateReportedTransactionException>(() => transactionService.ValidateTransaction(transactionModel, now));
        }
        
        [Fact]
        public void ValidateTransaction_Should_Return_TransactionEntity_When_Transaction_Is_Valid()
        {
            var now = DateTime.UtcNow;
            var transactionModel = new TransactionModel
            {
                Amount = "123.14",
                Timestamp = now.ToString(ServiceConstants.DateTimeFormat)
            };

            var transactionService = _fixture.Create<ValidatorService>();

            var transactionEntity = transactionService.ValidateTransaction(transactionModel, now);
            
            Assert.Equal(now.Year, transactionEntity.Timestamp.Year);
            Assert.Equal(now.Month, transactionEntity.Timestamp.Month);
            Assert.Equal(now.Day, transactionEntity.Timestamp.Day);
            Assert.Equal(now.Hour, transactionEntity.Timestamp.Hour);
            Assert.Equal(now.Minute, transactionEntity.Timestamp.Minute);
            Assert.Equal(now.Second, transactionEntity.Timestamp.Second);
            Assert.Equal(now.Millisecond, transactionEntity.Timestamp.Millisecond);
            Assert.Equal(decimal.Parse(transactionModel.Amount, CultureInfo.InvariantCulture), transactionEntity.Amount);
        }
    }
}