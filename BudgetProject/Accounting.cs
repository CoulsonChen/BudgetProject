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

            if (EndDate.Year - StartDate.Year == 0
                && EndDate.Month - StartDate.Month == 0)
            {
                double dayOMonth = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
                double dayDiff = (EndDate - StartDate).Days + 1;

                var ammount = budgetRepo.GetAll()
                    .FirstOrDefault(x => x.YearMonth == StartDate.ToString("yyyyMM")).Amount;

                return ammount * dayDiff / dayOMonth;
            }
            else
            {
                List<Budget> suBudgets = new List<Budget>();
                foreach (var obj in Budget)
                {
                    DateTime time = new DateTime(GetYear(obj.YearMonth),
                        GetMonth(obj.YearMonth), 1);
                    if (time.Year == StartDate.Year
                        && time.Month == StartDate.Month)
                    {
                        suBudgets.Add(obj);
                        continue;
                    }
                    else if (time.Year == EndDate.Year
                             && time.Month == EndDate.Month)
                    {
                        suBudgets.Add(obj);
                        continue;
                    }
                    if (time >= StartDate && time <= EndDate)
                    {
                        suBudgets.Add(obj);
                    }
                }

                double result = 0;
                foreach (var item in suBudgets)
                {
                    if (GetYear(item.YearMonth) == StartDate.Year
                        && GetMonth(item.YearMonth) == StartDate.Month)
                    {
                        int dayOfMonth = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
                        result += item.Amount * (dayOfMonth - StartDate.Day + 1) / dayOfMonth;
                        continue;
                    }
                    else if (GetYear(item.YearMonth) == EndDate.Year
                             && GetMonth(item.YearMonth) == EndDate.Month)
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
