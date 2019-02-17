using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Configuration;
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
            var Budgets = GetBudgetSet(StartDate, EndDate);

            if (!Budgets.Any())
                return 0;

            if (IsSameMonth(StartDate, EndDate))
            {
                double dayOMonth = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
                double dayDiff = (EndDate - StartDate).Days + 1;

                var ammount = budgetRepo.GetAll()
                    .FirstOrDefault(x => x.YearMonth == StartDate.ToString("yyyyMM")).Amount;

                return ammount * dayDiff / dayOMonth;
            }
            else
            {
                double result = 0;
                foreach (var item in Budgets)
                {
                    if (item.YearMonth == StartDate.ToString("yyyyMM"))
                    {
                        int dayOfMonth = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
                        result += item.Amount * (dayOfMonth - StartDate.Day + 1) / dayOfMonth;
                        continue;
                    }
                    else if (item.YearMonth == EndDate.ToString("yyyyMM"))
                    {
                        int dayOfMonth = DateTime.DaysInMonth(EndDate.Year, EndDate.Month);
                        result += item.Amount * EndDate.Day / dayOfMonth;
                        continue;
                    }
                    else
                    {
                        result += item.Amount;
                    }


                }

                return result;
            }

        }
        
        private List<Budget> GetBudgetSet(DateTime StartDate, DateTime EndDate)
        {
            var allBudget = budgetRepo.GetAll();
            var budgetTimeStampList = GetBudgetTimeStampList(StartDate, EndDate);

            List<Budget> returnBudgets = allBudget.Where(x => budgetTimeStampList.Contains(x.YearMonth)).ToList();

            return returnBudgets;
        }

        private List<string> GetBudgetTimeStampList(DateTime StartDate, DateTime EndDate)
        {
            DateTime timeFlag = StartDate;
            List<string> returnTimeStamps = new List<string>();

            while (!IsSameMonth(timeFlag, EndDate))
            {
                returnTimeStamps.Add(timeFlag.ToString("yyyyMM"));
                timeFlag = timeFlag.AddMonths(1);
            }
            returnTimeStamps.Add(timeFlag.ToString("yyyyMM"));

            return returnTimeStamps;
        }

        private bool IsSameMonth(DateTime StartDate, DateTime EndDate)
        {
            if (EndDate.Year - StartDate.Year == 0
                && EndDate.Month - StartDate.Month == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
