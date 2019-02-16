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
            if (StartDate.Equals(EndDate))
            {
                return budgetRepo.GetAll()
                    .FirstOrDefault(x => x.YearMonth == StartDate.ToString("yyyyMM")).Amount
                       / DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
            }
            return budgetRepo.GetAll()
                .FirstOrDefault(x => x.YearMonth == StartDate.ToString("yyyyMM")).Amount;
        }
    }
}
