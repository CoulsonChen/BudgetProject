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
        private FakeBudgetRepo _budgetRepo = new FakeBudgetRepo();
        private Accounting _accounting;

        [TestInitialize]
        public void TestInit()
        {
            _accounting = new Accounting(_budgetRepo);
        }

        [TestMethod]
        public void AMonth()
        {
            // Prepare
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 31);
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62}
            };

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(62, total);
        }

        [TestMethod]
        public void SingleDate()
        {
            // Prepare
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 01);
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62},
            };

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(2, total);
        }

        [TestMethod]
        public void UnvalidDatetime()
        {
            // Prepare
            var startDate = new DateTime(2018, 4, 01);
            var endDate = new DateTime(2018, 3, 01);
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget() {YearMonth = "201803", Amount = 0}
            };

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(0, total);
        }

        [TestMethod]
        public void NoData()
        {
            // Prepare
            var startDate = new DateTime(2018, 4, 01);
            var endDate = new DateTime(2018, 4, 02);
            _budgetRepo.ReturnBudgets = new List<Budget>() { };

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(0, total);
        }

        [TestMethod]
        public void AccrossMonth()
        {
            // Prepare
            var startDate = new DateTime(2018, 1, 31);
            var endDate = new DateTime(2018, 2, 1);
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201801", Amount = 62},
                new Budget(){YearMonth = "201802", Amount = 28}
            };

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
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201802", Amount = 28},
                new Budget(){YearMonth = "201803", Amount = 0}
            };

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
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201801", Amount = 62},
                new Budget(){YearMonth = "201802", Amount = 28},
                new Budget(){YearMonth = "201803", Amount = 0}
            };

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
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201803", Amount = 0},
                new Budget(){YearMonth = "201805", Amount = 31}
            };

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
            _budgetRepo.ReturnBudgets = new List<Budget>()
            {
                new Budget(){YearMonth = "201801", Amount = 62}
            };

            // Act
            double total = _accounting.TotalAccoount(startDate, endDate);

            // Assert
            Assert.AreEqual(4, total);
        }

        public class FakeBudgetRepo : BudgetRepo
        {
            public List<Budget> ReturnBudgets;
            public override List<Budget> GetAll()
            {
                return ReturnBudgets;
            }
        }

    }
}
