namespace ATMmachine.Models
{
    using System;

    public class Fee
    {
        public struct CardNumber { }
        public decimal WithdrawalFeeAmount { get; set; }
        public DateTime WithdrawalDate { get; set; }
    }
}
