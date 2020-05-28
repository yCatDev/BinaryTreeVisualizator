using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Nez;
using Microsoft.Xna.Framework.Graphics;
using TinyAlgorithmVisualizer.Scenes;

namespace TinyAlgorithmVisualizer
{
    public class MyGame : Core
    {
        protected override void Initialize()
        {
            base.Initialize();
            Screen.SetSize(1280, 800);
            DefaultSamplerState = SamplerState.AnisotropicClamp;
            Scene = new BlankScene();
        }
    }
}