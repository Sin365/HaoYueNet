using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HaoYueNet.ClientNetwork.NetworkHelperCore;

namespace ClientCore.Event
{
    public class DelegateClass
    {
        public delegate void dg_Str(string Msg);
        public delegate void dg_Str_Str(string Str1, string Str2);
    }
}
