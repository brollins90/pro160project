using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public interface IMovingObject
    {
        void Move(Point destination);
    }
}
