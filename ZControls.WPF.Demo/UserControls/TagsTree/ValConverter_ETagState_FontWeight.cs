using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ZControls.WPF.Demo.DataModel;

namespace ZControls.WPF.Demo.UserControls
{
    class ValConverter_ETagState_FontWeight : IValueConverter
    {
        private static readonly Dictionary<ETagState, FontWeight> ETagState_Brush = new Dictionary<ETagState, FontWeight>
        {
            { ETagState.Exclude, FontWeights.Bold },
            { ETagState.Include, FontWeights.Bold },
            { ETagState.Undefined, FontWeights.Normal },
            { ETagState.Unavailable, FontWeights.Normal }
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
