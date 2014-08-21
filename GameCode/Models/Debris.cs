﻿using GameCode.Helpers;
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
        public Debris(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
        }
        public override void Update(double deltaTime)
        {
            return;
        }
    }
}