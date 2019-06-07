using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public static class CalculationHelper
    {
        public static Double HowPercentIsXFromY(Double x, Double y)
        {
            return y / x;
        }

        public static Double CalculatePercent(Double unitPrice, Double priceAfterDisccount)
        {
            Double x = unitPrice;
            Double y = priceAfterDisccount;
            Double z = unitPrice - priceAfterDisccount;

            return (z / x) * 100;
        }
    }
}
