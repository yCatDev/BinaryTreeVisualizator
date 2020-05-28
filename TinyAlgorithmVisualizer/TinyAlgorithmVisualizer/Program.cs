using System;

namespace TinyAlgorithmVisualizer
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