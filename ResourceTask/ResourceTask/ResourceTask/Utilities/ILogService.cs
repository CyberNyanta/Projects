using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTask.Core.Utilities
{
    public interface ILogService
    {
        void Exception(object sender, Exception ex);
        void Info(object sender, string message, params object[] parameters);
        void Warning(object sender, string warning, params object[] parameters);
    }
}
