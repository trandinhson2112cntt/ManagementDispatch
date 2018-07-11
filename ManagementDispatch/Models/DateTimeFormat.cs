using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ManagementDispatch.Models
{
    public class DateTimeFormat
    {
        void abc()
        {
            IFormatProvider culture = new CultureInfo("vi-VN", true);
            string timeDayFormat = "dd/MM/yyyy";
            string timeHourFormat = "HH:mm:ss";
            string timeDayHourFormat = "dd/MM/yyyy HH:mm:ss";

            DateTime d = DateTime.ParseExact("25 / 02 / 2010", timeDayFormat, culture);
        }
        
    }
}