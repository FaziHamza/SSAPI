using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSAPI.Core
{
    public interface IEndpointReader
    {
        Task ReadAsync();
    }

}
