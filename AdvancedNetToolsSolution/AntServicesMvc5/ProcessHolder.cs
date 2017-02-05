using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AntServicesMvc5
{
    public static class ProcessHolder
    {
        public static Dictionary<string, Process> holder = new Dictionary<string, Process>();

        public static Dictionary<string, Process> Instance
        {
            get
            {
                return holder;
            }
        }
    }
}