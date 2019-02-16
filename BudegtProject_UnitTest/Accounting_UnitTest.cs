﻿using System;
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

        [TestMethod]
        public void AMonth()
        {
            var startDate = new DateTime(2018, 1, 01);
            var endDate = new DateTime(2018, 1, 31);
            _budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "201801", Amount = 62},
                new Budget() {YearMonth = "201802", Amount = 28},
                new Budget() {YearMonth = "201803", Amount = 0},
                new Budget() {YearMonth = "201805", Amount = 31},
            });
            var accounting = new Accounting(_budgetRepo);
            Assert.AreEqual(62, accounting.TotalAccoount(startDate, endDate));
        }

        [Ignore]
        [TestMethod]
        public void UnvalidDatetime()
        {
            //BudgetShouldBe(0, new DateTime(2018, 04, 01), new DateTime(2018, 03, 01));
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
