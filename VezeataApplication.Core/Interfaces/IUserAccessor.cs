using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeataApplication.Core.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUsername();
        int? GetLoggedInId();
    }
}
