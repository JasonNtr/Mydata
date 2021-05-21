using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Helpers
{
    public static class Helper
    {

        public static bool areSmallDatesEquals(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null)
            {
                return false;
            }

            var date1notnull = (DateTime)date1;
            var date2notnull = (DateTime)date2;
            var areEquals = (date1notnull.Year == date2notnull.Year && date1notnull.Month == date2notnull.Month && date1notnull.Day == date2notnull.Day);
            return areEquals;
        }


    }
}
