using System;
using System.Threading.Tasks;
using Core.Constants;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        
        [HttpPost]
        [Route(ApiConstants.EmptyRoot)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] TransactionModel transaction)
        {
            var now = DateTime.UtcNow;
            
            _transactionService.CreateTransaction(transaction, now);

            return StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpGet]
        [Route(ApiConstants.Statistics)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStats()
        {
            var now = DateTime.UtcNow;
            
            var stats = _transactionService.GetStats(now);

            return Ok(stats);
        }
        
        [HttpDelete]
        [Route(ApiConstants.EmptyRoot)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTransactions()
        {
            _transactionService.DeleteTransactions();

            return NoContent();
        }
    }
}