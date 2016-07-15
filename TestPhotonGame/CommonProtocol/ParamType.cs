using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProtocol
{
    public enum LoginParamType : byte
    {
        Account = 1,
        Password,
        NickName,
        Ret = 80
    }
}
