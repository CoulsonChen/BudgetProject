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
            var Budget = GetBudgetSet(StartDate, EndDate);

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
                returnTimeStamps.Add(string.Format("{0}{1}"
                    , timeFlag.Year.ToString(), timeFlag.Month.ToString("d2")));

                timeFlag = timeFlag.AddMonths(1);
            }
            returnTimeStamps.Add(string.Format("{0}{1}"
                , EndDate.Year.ToString(), EndDate.Month.ToString("d2")));

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
