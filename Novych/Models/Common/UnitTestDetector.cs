using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Novych.Models.Common
{
    public static class UnitTestDetector
    {
        private static bool _runningFromNUnit = false;

        static UnitTestDetector()
        {
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                System.Diagnostics.Debug.WriteLine("assem.FullName - " + assem.FullName);

                if (assem.FullName.StartsWith("Microsoft.VisualStudio.TestPlatform", StringComparison.InvariantCultureIgnoreCase))
                {
                    _runningFromNUnit = true;
                    break;
                }
            }
        }

        public static bool IsRunningFromNUnit
        {
            get { return _runningFromNUnit; }
        }
    }
}