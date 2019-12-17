using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using ZControls.WPF.Demo.DataModel;

namespace ZControls.WPF.Demo.UserControls
{
    public class ValConverter_EtagState_Brush : IValueConverter
    {
        private static readonly Dictionary<ETagState, Brush> ETagState_Brush = new Dictionary<ETagState, Brush>
        {
            { ETagState.Exclude, Brushes.Red },
            { ETagState.Include, Brushes.Green },
            { ETagState.Undefined, Brushes.SteelBlue },
            { ETagState.Unavailable, Brushes.Gray }
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(ETagState)) return null;
            return ETagState_Brush[(ETagState)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
