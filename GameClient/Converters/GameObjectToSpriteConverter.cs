using GameCode.Models;
using GameCode.Models.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameClient.Converters
{
    public class GameObjectToSpriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GameObject current = (GameObject)value;
            Uri picture = null;

            var typeSwitch = new Dictionary<Type, Action> {
                { typeof(Character), () => picture = new Uri("Images/ArcherCharacter.png", UriKind.Relative) },
                { typeof(Bot), () => {
                    Bot temp = (Bot)current;
                    switch (temp.BotClass)
                    {
                        case BotClass.Boss:
                            picture = new Uri("Images/BossTopView.png", UriKind.Relative);
                            break;
                        case BotClass.Melee:
                            picture = new Uri("Images/Minion.png", UriKind.Relative);
                            break;
                        case BotClass.Mercenary:
                            picture = new Uri("Images/MercenaryTopView.png", UriKind.Relative);
                            break;
                        case BotClass.Shooter:
                            picture = new Uri("Images/Minion.png", UriKind.Relative);
                            break;
                        case BotClass.Tower:
                            picture = new Uri("Images/TowerCastleThingTopView.png", UriKind.Relative);
                            break;
                        case BotClass.Turret:
                            picture = new Uri("Images/SentryTopView.png", UriKind.Relative);
                            break;
                    }
                } },
                { typeof(Arrow), () => picture = new Uri("Images/BallistaArrow.png", UriKind.Relative) },
                { typeof(Debris), () => picture = new Uri("Images/BrickWall.png", UriKind.Relative) }
            };

            typeSwitch[current.GetType()]();

            ImageSource image = new BitmapImage(picture);
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
