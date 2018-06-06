using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Core.Model;

namespace TaskManager.Core.Exchange
{
    public interface IRateGatter
    {
        Task<IEnumerable<ExchangeRate>> GetRates();
    }
}