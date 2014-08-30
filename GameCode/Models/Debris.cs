﻿using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public class Debris : GameObject
    {
        public Debris(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
            Team = GameConstants.TEAM_INT_DEBRIS;
            ClassType = GameConstants.TYPE_DEBRIS_BUSH;
        }
        public override void Update(double deltaTime)
        {
            return;
        }
    }
}
