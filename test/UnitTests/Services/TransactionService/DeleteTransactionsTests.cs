using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Repository.Contracts;
using Xunit;

namespace UnitTests
{
    public class DeleteTransactionsTests
    {
        private readonly Fixture _fixture;

        public DeleteTransactionsTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        }
        
        [Fact]
        public void DeleteTransactions_Should_Call_DeleteTransactions_Method_Of_TransactionRepository()
        {
            var transactionRepository = _fixture.Freeze<Mock<ITransactionRepository>>();
            transactionRepository.Setup(m => m.DeleteTransactions()).Verifiable();

            var transactionService = _fixture.Create<Service.Implementations.TransactionService>();

            transactionService.DeleteTransactions();
            
            transactionRepository.Verify(m => m.DeleteTransactions(), Times.Once);
        }
    }
}