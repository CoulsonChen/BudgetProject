using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BudgetProject;

namespace BudegtProject_UnitTest
{
    [TestClass]
    public class Accounting_UnitTest
    {
        Accounting accounting = new Accounting();

        

        private void BudgetShouldBe(double Budget, DateTime StartDate, DateTime EndDate)
        {
            Assert.AreEqual(Budget, accounting.TotalAccoount(StartDate, EndDate));
        }
    }
}
