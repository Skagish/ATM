namespace AtmUnitTests.ServicesTests
{
    using ATMmachine.Enums;
    using ATMmachine.Models;
    using ATMmachine.Services;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    class ATMachineTests
    {
        private Mock<ATMachine> _atmMock;

        [SetUp]
        public void Setup()
        {
            _atmMock = new Mock<ATMachine>();
        }

        [Test]
        public void ShouldThrowErrorOnWithdrawalOnLowerAtmBalance()
        {
            _atmMock.Object.InsertCard("1234 1234 1234 1234");
            Assert.That(() => _atmMock.Object.WithdrawMoney(1105), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ShouldThrowErrorOnWithdrawalWithoutCard()
        {
            Assert.That(() => _atmMock.Object.WithdrawMoney(20), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ShouldReturnCorrectNotesOnSmallWithdrawal()
        {
            Money expectedMoney = new Money()
            {
                Amount = 75,
                Notes = new Dictionary<PaperNote, int>() {
                { PaperNote.FiftyEuro, 1},
                { PaperNote.TwentyEuro, 1 },
                { PaperNote.FiveEuro, 1 }
            }};

            _atmMock.Object.InsertCard("1234 1234 1234 1234");
            var returnedMoney = _atmMock.Object.WithdrawMoney(75);
            Assert.AreEqual(returnedMoney.Notes, expectedMoney.Notes);
        }

        [Test]
        public void ShouldReturnCorrectNotesOnMaxWithdrawal()
        {
            Money expectedMoney = new Money()
            {
                Amount = 1100,
                Notes = new Dictionary<PaperNote, int>() {
                { PaperNote.FiftyEuro, 10},
                { PaperNote.TwentyEuro, 15 },
                { PaperNote.TenEuro, 20 },
                { PaperNote.FiveEuro, 20 }
            }};

            _atmMock.Object.InsertCard("1234 1234 1234 1234");
            var returnedMoney = _atmMock.Object.WithdrawMoney(1100);
            Assert.AreEqual(returnedMoney.Notes, expectedMoney.Notes);
        }

        [Test]
        public void ShouldThrowErrorOnBalanceWithoutCard()
        {
            Assert.That(() => _atmMock.Object.GetCardBalance(), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldReturnBalance()
        {
            _atmMock.Object.InsertCard("1234 1234 1234 1234");
            var balance = _atmMock.Object.GetCardBalance();
            Assert.IsTrue(balance >= 5);
        }

        [Test]
        public void ShouldThrowErrorOnCardInsertWhileCardInside()
        {
            _atmMock.Object.InsertCard("1234 1234 1234 1234");
            Assert.That(() => _atmMock.Object.InsertCard(It.IsAny<string>()), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldThrowErrorOnReturnCardWithNoCardInside()
        {
            Assert.That(() => _atmMock.Object.ReturnCard(), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldLoadMoneyInAtm()
        {
            Money loadedMoney = new Money()
            {
                Amount = 75,
                Notes = new Dictionary<PaperNote, int>() {
                { PaperNote.FiftyEuro, 1},
                { PaperNote.TwentyEuro, 1 },
                { PaperNote.FiveEuro, 1 }
            }};

            _atmMock.Object.InsertCard("1234 1234 1234 1234");
            var startingBalance = _atmMock.Object.GetCardBalance();
            _atmMock.Object.LoadMoney(loadedMoney);
            Assert.Greater(_atmMock.Object.GetCardBalance(), startingBalance);
        }
    }
}
