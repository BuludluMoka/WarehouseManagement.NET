﻿namespace Warehouse.Data.Dto.Transactions
{
    public class AdminTransactionCreateDto
    {
        
        public int? sender_id { get; set; }
        public int receiver_id { get; set; }
        public int ProductId { get; set; }

        public int Count { get; set; }
    }
}
