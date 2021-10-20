using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace SilupostMobileApp.Common
{
    public class Converters
    {
    }
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (date.ToString("MMMM/yyyy").Equals(DateTime.Today.ToString("MMMM/yyyy")))
            {
                return "This Month";
            }
            return date.ToString("MMM yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (date.Equals(DateTime.Today))
            {
                return "Today";
            }
            return date.ToString("MMM dd, yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //DateTime date = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToString("MM/dd/yyyy"), value.ToString().Remove(5).ToString()));
            //DateTime date = DateTime.ParseExact(value.ToString().Remove(5).ToString(), "hh:mm", null, System.Globalization.DateTimeStyles.None);
            var timeArray = value.ToString().Split(new string[] { ":", " ", "." }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date  = DateTime.ParseExact(string.Format("{0}:{1}", timeArray[0].PadLeft(2, '0'), timeArray[1].PadLeft(2, '0')), "hh:mmtt", null, System.Globalization.DateTimeStyles.None);
            return date.ToString("hh:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToString("MM/dd/yyyy"), value.ToString().Remove(5).ToString()));
            return date.TimeOfDay;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class FirstValidationErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorList = (List<string>)value;
            var error = string.Empty;
            if (errorList != null && errorList.Count > 0)
                error = errorList.FirstOrDefault();
            return error.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
