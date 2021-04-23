namespace ATMmachine.Models
{
    using ATMmachine.Enums;
    using System.Collections.Generic;

    public struct Money
    {
        public int Amount { get; set; }
        public Dictionary<PaperNote, int> Notes { get; set; }
    }
}
