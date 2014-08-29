using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace GameCode.Models
{
    public class Bushes : Debris
    {
        public Bushes(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
        }
    }
}
