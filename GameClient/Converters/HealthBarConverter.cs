﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GameClient.Converters
{
    public class PlayerHealthBarConverterWidth : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is int && values[1] is int)
            {
                double d1 = (int)values[0];
                double d2 = (int)values[1];
                double result = (double)(d1 / d2) * 200;
                return (result < 0) ? 0 : result;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PlayerXPBarConverterWidth : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is int && values[1] is int)
            {
                double d1 = (int)values[0];
                double d2 = (int)values[1];
                double result = (double)(d1 / d2) * 580;
                return (result < 0) ? 0 : result;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BotHealthBarConverterWidth : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is int && values[1] is int)
            {
                double d1 = (int)values[0];
                double d2 = (int)values[1];
                double result = (double)(d1 / d2) * 60;
                return (result < 0) ? 0 : result;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BotHealthBarConverterX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                double yval = (double)value;
                return yval + 10;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BotHealthBarConverterY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double yval = (double)value;
            return yval - 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
