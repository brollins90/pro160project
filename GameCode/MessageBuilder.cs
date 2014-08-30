using GameCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode
{
    public static class MessageBuilder
    {

        internal static string AddMessage(GameObject o)
        {
            String msgString = "";// +GameConstants.MSG_ADD + "," + o.ClassType + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Velocity.x + "," + o.Velocity.y + "," + o.Velocity.z + "," + o.Angle;
            if (o.ClassType > GameConstants.TYPE_DEBRIS_LOW && o.ClassType < GameConstants.TYPE_DEBRIS_HIGH)
            {
                // ADD Debris: MSG_ADD, ClassType, ID, PosX, PosY, PosZ, SizeX, SizeY, SizeZ, Ang
                msgString = "" + GameConstants.MSG_ADD + "," + o.ClassType + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Size.x + "," + o.Size.y + "," + o.Size.z + "," + o.Angle;
            }
            else if (o.ClassType > GameConstants.TYPE_CHARACTER_LOW && o.ClassType < GameConstants.TYPE_CHARACTER_HIGH)
            {
                // ADD Character: MSG_ADD, ClassType, ID, PosX, PosY, PosZ, VelX, VelY, VelZ, Ang, Damage, 
                Character c = ((Character)o);
                msgString = "" + GameConstants.MSG_ADD + "," + c.ClassType + "," + c.ID + "," + c.Position.x + "," + c.Position.y + "," + c.Position.z + "," + c.Size.x + "," + c.Size.y + "," + c.Size.z + "," + c.Angle;// +"," + c.Damage;
            }
            else
            {
                // ADD Other (Bot/Proj): MSG_ADD, ClassType, ID, PosX, PosY, PosZ, VelX, VelY, VelZ, Ang
                MovingObject b = (MovingObject)o;
                msgString = "" + GameConstants.MSG_ADD + "," + b.ClassType + "," + b.ID + "," + b.Position.x + "," + b.Position.y + "," + b.Position.z + "," + b.Velocity.x + "," + b.Velocity.y + "," + b.Velocity.z + "," + b.Angle;
            }
            return msgString;
        }

        internal static string RequestAllMessage()
        {
            string msgString = "" + GameConstants.MSG_REQUEST_ALL_DATA + ",0,0";
            return msgString;
        }

        internal static string DeadMessage(GameObject o)
        {
            String msgString = "" + GameConstants.MSG_DEAD + "," + o.ClassType + "," + o.ID + ",0,0,0,0,0,0,0";
            return msgString;
        }

        internal static string AttackMessage(Character character, int projID)
        {
            string msgString = "" + GameConstants.MOVEMENT_ATTACK + "," + character.Weapon.Projectile.ClassType + "," + projID + "," + character.Position.x + "," + character.Position.y + "," + character.Position.z + ",0,0,0," + character.Angle + "," + character.ID;
            return msgString;
        }

        internal static string UpdateMessage(MovingObject o)
        {
            String msgString = "" + GameConstants.MSG_UPDATE + "," + o.ClassType + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Velocity.x + "," + o.Velocity.y + "," + o.Velocity.z + "," + o.Angle;
            return msgString;
        }
    }
}
