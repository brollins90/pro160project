using GameCode;
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
    public class SpriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GameObject current = (GameObject)value;
            Uri picture = null;

            var typeSwitch = new Dictionary<Type, Action> {
                { typeof(Character), () => picture = new Uri("Images/ArcherCharacter.png", UriKind.Relative) },
                { typeof(Bot), () => {
                    Bot temp = (Bot)current;
                    switch (temp.ClassType)
                    {
                        case GameConstants.TYPE_BOT_BOSS:
                            picture = new Uri("Images/BossTopView.png", UriKind.Relative);
                            break;
                        case GameConstants.TYPE_BOT_MELEE:
                            picture = new Uri("Images/Minion.png", UriKind.Relative);
                            break;
                        case GameConstants.TYPE_BOT_MERCENARY:
                            picture = new Uri("Images/MercenaryTopView.png", UriKind.Relative);
                            break;
                        case GameConstants.TYPE_BOT_SHOOTER:
                            picture = new Uri("Images/Minion.png", UriKind.Relative);
                            break;
                        case GameConstants.TYPE_BOT_TOWER:
                            picture = new Uri("Images/TowerCastleThingTopView.png", UriKind.Relative);
                            break;
                        case GameConstants.TYPE_BOT_TURRET:
                            picture = new Uri("Images/SentryTopView.png", UriKind.Relative);
                            break;
                    }
                } },
                { typeof(Arrow), () => picture = new Uri("Images/BallistaArrow2.0.png", UriKind.Relative) },
                { typeof(StabAttack), () => picture = new Uri("Images/MasterSwordReplica.png", UriKind.Relative) },
                { typeof(FireBall), () => picture = new Uri("Images/FireBall.png", UriKind.Relative) },
                { typeof(Bushes), () => picture = new Uri("Images/ItsATree.png", UriKind.Relative) },
                { typeof(Rocks), () => picture = new Uri("Images/RockPower.png", UriKind.Relative) },
                { typeof(CastleWalls), () => picture = new Uri("Images/BrickWall2.0.png", UriKind.Relative) },
                { typeof(Debris), () => picture = new Uri("Images/BrickWall2.0.png", UriKind.Relative) }
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
