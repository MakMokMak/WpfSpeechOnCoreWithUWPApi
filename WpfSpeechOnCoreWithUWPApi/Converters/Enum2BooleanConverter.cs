using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfSpeechOnCoreWithUWPApi.Converters
{
    /// <summary>
    /// 列挙型とブーリアン型との間の変換をバインディングに適用します。
    /// </summary>
    public class Enum2BooleanConverter : IValueConverter
    {
        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値</param>
        /// <param name="targetType">バインディング ターゲット プロパティの値</param>
        /// <param name="parameter">XAML の ConverterParameter で記述された値</param>
        /// <param name="culture">使用するカルチャ</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString))
            {
                return DependencyProperty.UnsetValue;
            }

            if (!Enum.IsDefined(value.GetType(), value))
            {
                return DependencyProperty.UnsetValue;
            }

            var parameterValue = Enum.Parse(value.GetType(), parameterString);
            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString))
            {
                return DependencyProperty.UnsetValue;
            }

            if (true.Equals(value))
            {
                return Enum.Parse(targetType, parameterString);
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
