using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace GameClient.Converters
{
    public class CharImgConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri picture = null;
            GameCode.Models.BotClass gen = (GameCode.Models.BotClass)values[0];
            if (gen == GameCode.Models.BotClass.Melee)
            {

            }
            else if (gen == GameCode.Models.BotClass.Boss)
            {

            }
            else if (gen == GameCode.Models.BotClass.Shooter)
            {

            }
            else if (gen == GameCode.Models.BotClass.Mercenary)
            {

            }
            else if (gen == GameCode.Models.BotClass.Turret)
            {

            }
            else if (gen == GameCode.Models.BotClass.Tower)
            {

            }

            ImageSource image = new BitmapImage(picture);
            return image;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
