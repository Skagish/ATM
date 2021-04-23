namespace ATMmachine.Controllers
{
    using System;
    using ATMmachine.Models;
    using ATMmachine.Services;
    using Microsoft.AspNetCore.Mvc;

    public class AtmController : Controller
    {
        private readonly Random _random = new Random();
        private readonly IATMachine _atm;

        public AtmController(IATMachine atm)
        {
            _atm = atm;
        }

        // GET: ATM/ViewBalance
        [HttpGet("ATM/StarScreen")]
        public ActionResult StartScreen()
        {
            return View();
        }

        // GET: ATM/ViewBalance
        [HttpGet("ATM/InsertCard")]
        public ActionResult InsertCard()
        {
            try
            {
                //Card number input mocked for testing
                _atm.InsertCard($"{_random.Next(1000, 9999)} {_random.Next(1000, 9999)} {_random.Next(1000, 9999)} {_random.Next(1000, 9999)}");
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: ATM/ViewBalance
        [HttpGet("ATM/ReturnCard")]
        public ActionResult ReturnCard()
        {
            try
            {
                _atm.ReturnCard();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: ATM/ViewBalance
        [HttpGet("ATM/ViewBalance")]
        public string ViewBalance()
        {
            try
            {
                return $"{_atm.GetCardBalance().ToString("F")}$";
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("ATM/AddBalance")]
        public ActionResult AddBalance(Money amount)
        {
            try
            {
                _atm.LoadMoney(amount);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ATM/Withdraw/{amount}")]
        public ActionResult<Money> Withdraw([FromRoute]int amount)
        {
            try
            {
                return _atm.WithdrawMoney(amount);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}