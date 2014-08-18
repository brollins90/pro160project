using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{
    public class Debris : GameObject
    {
        public Debris(Vector position, GameManager manager, int width, int height)
            : base(position, manager, width:width, height:height)
        {
            this.Position = new Vector(400, 500);
            this.Width = 30;
            this.Height = 100;
        }
        public override void Update(int deltaTime)
        {
            return;
        }
    }
}
