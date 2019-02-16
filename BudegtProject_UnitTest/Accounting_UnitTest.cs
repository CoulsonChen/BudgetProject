using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BudgetProject;
using NSubstitute;

namespace BudegtProject_UnitTest
{
    [TestClass]
    public class Accounting_UnitTest
    {


        [TestMethod]
        public void AMonth()
        {
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 31);
            var models = new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62} };
            var accountinng = CreateAccounting(models);
            Assert.AreEqual(62, accountinng.TotalAccoount(startDate, endDate));
        }
        [TestMethod]
        public void SingleDate()
        {
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 01);
            var models = new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62},
            };
            var accountinng = CreateAccounting(models);
            Assert.AreEqual(2, accountinng.TotalAccoount(startDate, endDate));
        }
        private Accounting CreateAccounting(IEnumerable<Budget> list)
        {
            var _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetRepo.GetAll().Returns(list);
            return new Accounting(_budgetRepo);
        }

        [TestMethod]
        public void UnvalidDatetime()
        {
            var startDate = new DateTime(2018, 4, 01);
            var endDate = new DateTime(2018, 3, 01);
            var models = new List<Budget>()
            {
                new Budget() {YearMonth = "201803", Amount = 0}
            };
            var accountinng = CreateAccounting(models);
            Assert.AreEqual(0, accountinng.TotalAccoount(startDate, endDate));
        }

        [Ignore]
        [TestMethod]
        public void NoData()
        {
            //BudgetShouldBe(0, new DateTime(2018, 04, 01), new DateTime(2018, 04, 02));
        }

        private static IBudgetRepo GetBudgetRepo()
        {
            IBudgetRepo budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();

            return budgetRepo;
        }

    }
}
