using System;
using System.Collections.Generic;
using System.Text;

namespace Mydata.Helpers
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
