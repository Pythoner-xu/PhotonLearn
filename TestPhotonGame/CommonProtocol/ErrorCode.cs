using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProtocol
{
    public enum ErrorCode : byte
    {
        Ok = 0,
        InvalidOperation,
        InvalidParam
    }
}
