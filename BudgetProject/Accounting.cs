using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetProject
{
    public class Accounting
    {
        private IBudgetRepo budgetRepo;

        public Accounting(IBudgetRepo budgetRepo)
        {
            this.budgetRepo = budgetRepo;
        }

        public double TotalAccoount(DateTime StartDate, DateTime EndDate)
        {
            if (StartDate > EndDate) return 0;
            var Budget = budgetRepo.GetAll();
            if (!Budget.Any())
                return 0;

            double dayOMonth = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
            double dayDiff = (EndDate - StartDate).Days + 1;

            var ammount = budgetRepo.GetAll()
                .FirstOrDefault(x => x.YearMonth == StartDate.ToString("yyyyMM")).Amount;

            return ammount * dayDiff / dayOMonth;

            //return budgetRepo.GetAll()
            //    .FirstOrDefault(x => x.YearMonth == StartDate.ToString("yyyyMM")).Amount;

        }

        private int GetYear(string YearMonth)
        {
            return int.Parse(YearMonth.Substring(0, 4));
        }

        private int GetMonth(string YearMonth)
        {
            return int.Parse(YearMonth.Substring(4, 2));
        }
    }
}
