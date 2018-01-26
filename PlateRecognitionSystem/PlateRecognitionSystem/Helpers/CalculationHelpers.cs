using PlateRecognitionSystem.Enums;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Helpers
{
    public static class CalculationHelpers
    {
        public static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; i++)
                d[i, 0] = i;
            for (int j = 0; j <= m; j++)
                d[0, j] = j;

            for (int j = 1; j <= m; j++)
                for (int i = 1; i <= n; i++)
                    if (s[i - 1] == t[j - 1])
                        d[i, j] = d[i - 1, j - 1];  //no operation
                    else
                        d[i, j] = Math.Min(Math.Min(
                            d[i - 1, j] + 1,    //a deletion
                            d[i, j - 1] + 1),   //an insertion
                            d[i - 1, j - 1] + 1 //a substitution
                            );
            return d[n, m];
        }

        public static double CauntThePrice(TimeSpan visitTime,List<PriceRates> prices)
        {
            var minutes = visitTime.TotalMinutes;
            PriceEnum priceEnum = PriceEnum.OneHour; //cokolwiek
            double result = 0;
            prices.RemoveAll(x => x.Minutes == null);
            foreach (var price in prices)
            {
                var tempResult = minutes / (int)price.Minutes;
                if (result < tempResult)
                {
                    result = tempResult;
                    priceEnum = price.PriceEnum;
                } 
            }
            
            return Math.Ceiling(result) * (double)prices.SingleOrDefault(x => x.PriceEnum  == priceEnum).Cost;
        }
    }
}
