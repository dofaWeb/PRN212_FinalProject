using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.IO;

namespace PRN212_FinalProject.Helper
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Assuming `value` is the picture filename, e.g., "Lenovo.jpg"
            string filename = value as string;
            if (string.IsNullOrEmpty(filename))
                return null;

            // Set the relative path to the image folder
            return Path.Combine("Images", filename);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
