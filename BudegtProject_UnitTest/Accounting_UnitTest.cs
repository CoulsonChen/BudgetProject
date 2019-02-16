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
        private IBudgetRepo _budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
        private Accounting _accounting;
        [TestInitialize]
        public void TestInit()
        {
            _accounting = new Accounting(_budgetRepo);
        }

        [TestMethod]
        public void AMonth()
        {
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 31);
            var budgets = new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62} };


            BudgetSetup(budgets);
            Assert.AreEqual(62, _accounting.TotalAccoount(startDate, endDate));
        }
        [TestMethod]
        public void SingleDate()
        {
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 01);
            var budgets = new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62},
            };
            BudgetSetup(budgets);
            Assert.AreEqual(2, _accounting.TotalAccoount(startDate, endDate));
        }
        private void BudgetSetup(IEnumerable<Budget> list)
        {
            _budgetRepo.GetAll().Returns(list);
        }

        [TestMethod]
        public void UnvalidDatetime()
        {
            var startDate = new DateTime(2018, 4, 01);
            var endDate = new DateTime(2018, 3, 01);
            var budgets = new List<Budget>()
            {
                new Budget() {YearMonth = "201803", Amount = 0}
            };
             BudgetSetup(budgets);
            Assert.AreEqual(0, _accounting.TotalAccoount(startDate, endDate));
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
