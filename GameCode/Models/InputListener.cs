using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{
    public class InputListener
    {
        public bool KeyForward { get; set; }
        public bool KeyLeft { get; set; }
        public bool KeyBackward { get; set; }
        public bool KeyRight { get; set; }
        public bool KeyFire { get; set; }
        public bool MouseLeft { get; set; }
        public bool MouseRight { get; set; }
        public Point MousePos { get; set; }
    }
}
