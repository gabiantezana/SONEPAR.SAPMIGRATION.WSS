using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public static class ReleaseHelper
    {
        public static void FinalizeSAPObject(this Object o)
        {
            if (o != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            o = null;
            GC.Collect();
        }
    }
}
