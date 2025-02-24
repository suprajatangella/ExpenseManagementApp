using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagement.InfraStructure.Data
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
