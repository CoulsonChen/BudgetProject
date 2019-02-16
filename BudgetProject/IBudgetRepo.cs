using System.Collections.Generic;

namespace BudgetProject
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}