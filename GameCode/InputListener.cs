using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GameCode.Models
{
    public class InputListener
    {
        //private NetworkClient NetClient;
        public bool KeyForward;
        public bool KeyBackward;
        public bool KeyLeft;
        public bool KeyRight;
        public bool KeyAttack;
        public bool ESC;
        public bool F10;
        public bool F11;
        public Point MousePos;

        public InputListener()
        {
            //NetClient = netClient;
        }

        public void Gui_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("{0} InputListener - KeyDown: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, e.Key);
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    KeyForward = true;
                    break;

                case Key.Down:
                case Key.S:
                    KeyBackward = true;
                    break;

                case Key.Left:
                case Key.A:
                    KeyLeft = true;
                    break;

                case Key.Right:
                case Key.D:
                    KeyRight = true;
                    break;
                case Key.Escape:
                    ESC = true;
                    //Application.Current.Shutdown();
                    break;
                case Key.Space:
                case Key.T:
                    KeyAttack = true;
                    break;
            }
        }

        public void Gui_KeyUp(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("{0} InputListener - KeyUp: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, e.Key);
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    KeyForward = false;
                    break;

                case Key.Down:
                case Key.S:
                    KeyBackward = false;
                    break;

                case Key.Left:
                case Key.A:
                    KeyLeft = false;
                    break;

                case Key.Right:
                case Key.D:
                    KeyRight = false;
                    break;
                case Key.Escape:
                    ESC = true;
                    //Application.Current.Shutdown();
                    break;
                case Key.Space:
                case Key.T:
                    KeyAttack = false;
                    break;
                case Key.F10:
                    F10 = true;
                    break;
                case Key.F11:
                    F11 = true;
                    break;
            }
        }


        public void Gui_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("{0} InputListener - MouseDown: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, e.ChangedButton);
            if (e.ChangedButton == MouseButton.Left)
            {
                KeyAttack = true;
                //Console.WriteLine(e.GetPosition((IInputElement)sender));
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                //
            }
        }


        public void Gui_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("{0} InputListener - MouseDown: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, e.ChangedButton);
            if (e.ChangedButton == MouseButton.Left)
            {
                KeyAttack = false;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                //
            }
        }

        public void Gui_MouseMove(object sender, MouseEventArgs e)
        {
            MousePos = e.GetPosition((IInputElement)sender);
            //Console.WriteLine("{0} InputListener - MouseMove: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, MousePos);
        }
    }
}
