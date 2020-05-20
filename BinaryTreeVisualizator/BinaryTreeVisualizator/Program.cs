using System;

namespace BinaryTreeVisualizator
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new MyGame();
            game.Run();
        }
    }
}