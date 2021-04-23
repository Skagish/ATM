namespace ATMmachine.Services
{
    using ATMmachine.Enums;
    using ATMmachine.Models;
    using System;
    using System.Collections.Generic;

    public class ATMachine : IATMachine
    {
        private Money _atmBalance;
        private bool _cardInserted = false;
        private readonly Random _random = new Random();
        private decimal _cardBalance;

        string IATMachine.Manufacturer { get; set; } = "DeluxChoice";
        string IATMachine.SerialNumber { get; set; } = Guid.NewGuid().ToString();

        public ATMachine()
        {
            _cardBalance = _random.Next(5, 130);
            _atmBalance = new Money() { Amount = 1100, Notes = new Dictionary<PaperNote, int>() 
            {
                { PaperNote.FiftyEuro, 10},
                { PaperNote.TwentyEuro, 15},
                { PaperNote.TenEuro, 20},
                { PaperNote.FiveEuro, 20}
            }
            };
        }

        public decimal GetCardBalance()
        {
            if (_cardInserted)
            {
                return _cardBalance;
            }
            else
            {
                throw new Exception("Bank card is not inserted!");
            }
        }

        public void InsertCard(string cardNumber)
        {
            if (!_cardInserted)
            {
                _cardInserted = true;
            }
            else
            {
                throw new Exception("Bank card already inserted!");
            }
        }

        public void LoadMoney(Money money)
        {
            //TODO: Needs paper note validation
            AtmBalanceIncrease(money);
            _cardBalance += money.Amount;
        }

        private void AtmBalanceIncrease(Money money)
        {
            _atmBalance.Amount += money.Amount;
            foreach (var notesPerType in money.Notes)
            {
                _atmBalance.Notes[notesPerType.Key] += notesPerType.Value;
            }
        }

        public IEnumerable<Fee> RetrieveChargedFees()
        {
            throw new NotImplementedException();
        }

        public void ReturnCard()
        {
            if (_cardInserted)
            {
                _cardInserted = false;
            }
            else
            {
                throw new Exception("Bank card not inserted!");
            }
        }

        public Money WithdrawMoney(int amount)
        {
            bool valid = AmountValidation(amount);
            var atmBackup = _atmBalance;
            if (valid && _cardInserted && amount <= _atmBalance.Amount)
            {
                return GetPaperMoney(amount);
            }
            else
            {
                _atmBalance = atmBackup;
                throw new ArgumentException($"Cannot withdraw selected amount: {amount}");
            }
        }

        private Money GetPaperMoney(int amount)
        {
            var money = new Money() { Amount = 0, Notes = new Dictionary<PaperNote, int>()};
            var startingAmount = amount;
            while (amount >= 5)
            {
                if (amount >= 50 && AmountValidation(amount % 50) && _atmBalance.Notes[PaperNote.FiftyEuro] > 0)
                {
                    var paperNoteCount = (amount - (amount % 50)) / 50;
                    money = AtmBalanceDecrease(PaperNote.FiftyEuro, paperNoteCount, money, 50 * paperNoteCount);
                    amount = startingAmount - money.Amount;
                    continue;
                }
                else if (amount >= 20 && AmountValidation(amount % 20) && _atmBalance.Notes[PaperNote.TwentyEuro] > 0)
                {
                    var paperNoteCount = (amount - (amount % 20)) / 20;
                    money = AtmBalanceDecrease(PaperNote.TwentyEuro, paperNoteCount, money, 20 * paperNoteCount);
                    amount = startingAmount - money.Amount;
                    continue;
                }
                else if (amount >= 10 && AmountValidation(amount % 10) && _atmBalance.Notes[PaperNote.TenEuro] > 0)
                {
                    var paperNoteCount = (amount - (amount % 10)) / 10;
                    money = AtmBalanceDecrease(PaperNote.TenEuro, paperNoteCount, money, 10 * paperNoteCount);
                    amount = startingAmount - money.Amount;
                    continue;
                }
                else
                {
                    try
                    {
                        var paperNoteCount = (amount - (amount % 5)) / 5;
                        money = AtmBalanceDecrease(PaperNote.FiveEuro, paperNoteCount, money, 5*paperNoteCount);
                        amount = startingAmount - money.Amount;
                        continue;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Not enought paper notes in the ATM, please choose another value or ATM");
                    }
                }
            }
            return money;
        }

        private Money AtmBalanceDecrease(PaperNote paperNoteType, int paperNoteCount, Money money, int amount)
        {
            if (_atmBalance.Notes[paperNoteType] >= paperNoteCount)
            {
                _atmBalance.Amount -= amount;
                _atmBalance.Notes[paperNoteType] -= paperNoteCount;
                money.Amount += amount;
                if (money.Notes.ContainsKey(paperNoteType))
                {
                    money.Notes[paperNoteType] += paperNoteCount;
                }
                else
                {
                    money.Notes.Add(paperNoteType, paperNoteCount);
                }
                return money;
            }
            else if (_atmBalance.Notes[paperNoteType] > 0)
            {
                _atmBalance.Amount -= _atmBalance.Notes[paperNoteType] * (int)paperNoteType;
                money.Amount += _atmBalance.Notes[paperNoteType] * (int)paperNoteType;
                if (money.Notes.ContainsKey(paperNoteType))
                {
                    money.Notes[paperNoteType] += _atmBalance.Notes[paperNoteType];
                }
                else
                {
                    money.Notes.Add(paperNoteType, _atmBalance.Notes[paperNoteType]);
                }
                _atmBalance.Notes[paperNoteType] = 0;
                return money;
            }
            else
            {
                throw new Exception($"Not enough {paperNoteType} in the ATM");
            }
        }

        private bool AmountValidation(int amount)
        {
            switch (amount % 5)
            {
                case 0:
                    return true;
                default:
                    return false;
            }
        }
    }
}
