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
    public class CharacterSpriteConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri picture = null;
            GameCode.Models.GameObject.ObjectType gen = (GameCode.Models.GameObject.ObjectType)values[0];
            GameCode.Models.BotClass gen2 = (GameCode.Models.BotClass)values[1];
            if (gen == GameCode.Models.GameObject.ObjectType.Bot)
            {
                if (gen2 == GameCode.Models.BotClass.Melee)
                {
                    picture = new Uri("Images/Entei.png", UriKind.Relative);
                }
                else if (gen2 == GameCode.Models.BotClass.Boss)
                {
                    picture = new Uri("Images/Pikachu.png", UriKind.Relative);
                }
                else if (gen2 == GameCode.Models.BotClass.Shooter)
                {
                    picture = new Uri("Images/Charizard.png", UriKind.Relative);
                }
                else if (gen2 == GameCode.Models.BotClass.Mercenary)
                {
                    picture = new Uri("Images/Eevee.png", UriKind.Relative);
                }
                else if (gen2 == GameCode.Models.BotClass.Turret)
                {
                    picture = new Uri("Images/Mewtwo.png", UriKind.Relative);
                }
                else if (gen2 == GameCode.Models.BotClass.Tower)
                {
                    picture = new Uri("Images/Lucario.png", UriKind.Relative);
                }
            }
            else if (gen == GameCode.Models.GameObject.ObjectType.Bot)
            {
                picture = new Uri("Images/Red.png", UriKind.Relative);
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
