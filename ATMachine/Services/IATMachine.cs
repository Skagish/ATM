namespace ATMmachine.Services
{
    using ATMmachine.Models;
    using System.Collections.Generic;

    public interface IATMachine
    {
        string Manufacturer { get; set; }
        string SerialNumber { get; set; }
        void InsertCard(string cardNumber);
        decimal GetCardBalance();
        Money WithdrawMoney(int amount);
        void ReturnCard();
        void LoadMoney(Money money);
        IEnumerable<Fee> RetrieveChargedFees();
    }
}
