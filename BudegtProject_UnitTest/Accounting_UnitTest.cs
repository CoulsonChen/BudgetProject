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
        private IBudgetRepo _budgetRepo = Substitute.For<IBudgetRepo>();
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

        [TestMethod]
        public void NoData()
        {
            var startDate = new DateTime(2018, 4, 01);
            var endDate = new DateTime(2018, 4, 02);
            var budgets = new List<Budget>() { };
            BudgetSetup(budgets);
            Assert.AreEqual(0, _accounting.TotalAccoount(startDate, endDate));
        }
        
        [TestMethod]
        public void AccrossMonth()
        {
            // Prepare
            var startDate = new DateTime(2018, 1, 31);
            var endDate = new DateTime(2018, 2, 1);
            var budgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201801", Amount = 62},
                new Budget(){YearMonth = "201802", Amount = 28}
            };
            BudgetSetup(budgets);

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);
            
            // Assert
            Assert.AreEqual(3, total);
        }

        [TestMethod]
        public void AccrossMonth_WithZeroBudget()
        {
            // Prepare
            var startDate = new DateTime(2018, 2, 28);
            var endDate = new DateTime(2018, 3, 1);
            var budgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201802", Amount = 28},
                new Budget(){YearMonth = "201803", Amount = 0}
            };
            BudgetSetup(budgets);

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(1, total);
        }

        [TestMethod]
        public void AccrossSeveralMonth()
        {
            // Prepare
            var startDate = new DateTime(2018, 1, 31);
            var endDate = new DateTime(2018, 3, 1);
            var budgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201801", Amount = 62},
                new Budget(){YearMonth = "201802", Amount = 28},
                new Budget(){YearMonth = "201803", Amount = 0}
            };
            BudgetSetup(budgets);

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(30, total);
        }

        [TestMethod]
        public void AccrossMonth_WithNoData()
        {
            // Prepare
            var startDate = new DateTime(2018, 3, 1);
            var endDate = new DateTime(2018, 5, 1);
            var budgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201803", Amount = 0},
                new Budget(){YearMonth = "201805", Amount = 31}
            };
            BudgetSetup(budgets);

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(1, total);
        }

        [TestMethod]
        public void TwoDaysInOneMonth()
        {
            // Prepare
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 1, 2);
            var budgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201801", Amount = 62}
            };
            BudgetSetup(budgets);

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(4, total);
        }

        private void BudgetSetup(IEnumerable<Budget> list)
        {
            _budgetRepo.GetAll().Returns(list);
        }

    }
}
