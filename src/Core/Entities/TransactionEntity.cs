using System;

namespace Core.Entities
{
    public class TransactionEntity
    {
        public decimal Amount { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}