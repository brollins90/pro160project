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
                msgString = "" + GameConstants.MSG_ADD + "," + o.ClassType + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Size.x + "," + o.Size.y + "," + o.Size.z + "," + o.Angle;
            }
            else
            {
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
    }
}
