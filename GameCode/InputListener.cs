using System.Windows;
using System.Windows.Input;

namespace GameCode.Models
{
    public class InputListener
    {
        public bool KeyForward;
        public bool KeyBackward;
        public bool KeyLeft;
        public bool KeyRight;
        public bool KeyAttack;
        public bool ESC;
        public Point MousePos;

        public void Gui_KeyDown(object sender, KeyEventArgs e)
        {
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
                    break;

                case Key.Space:
                case Key.T:
                    KeyAttack = true;
                    break;
            }
        }

        public void Gui_KeyUp(object sender, KeyEventArgs e)
        {
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
                    break;

                case Key.Space:
                case Key.T:
                    KeyAttack = false;
                    break;
            }
        }


        public void Gui_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                KeyAttack = true;
            }
        }


        public void Gui_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                KeyAttack = false;
            }
        }

        public void Gui_MouseMove(object sender, MouseEventArgs e)
        {
            MousePos = e.GetPosition((IInputElement)sender);
        }
    }
}
