using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.GateWay.Models;

namespace Yarp.GateWay.Identity
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(Users request, out string token);
    }
}
