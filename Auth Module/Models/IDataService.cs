using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthModule.Models
{
    public interface IDataService
    {
        User isAuthenticated(User user);
    }
}
