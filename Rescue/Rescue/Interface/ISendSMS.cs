using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Rescue
{
    public interface ISendSMS
    {
        void Send(string number,string message);
    }
}
