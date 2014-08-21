﻿using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models.Projectiles
{
    public class Arrow : GameProjectile
    {
        public Arrow(Vector3 position, GameManager manager, double angle, int damage = 10, double rangeSquared = 200) :
            base(position, manager, new Vector3(10, 10, 0), angle, damage, rangeSquared)
        {

            }
    }
}
