using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Rescue
{
    public interface IToast
    {
        void Test();
        void Toasts(string function, string status);
    }
}
