using System;
using System.Runtime.InteropServices;
using BinaryTreeVisualizator.Tree;
using Microsoft.Xna.Framework;
using Nez;
using Microsoft.Xna.Framework.Graphics;

namespace BinaryTreeVisualizator
{
    public class MyGame : Core
    {
        protected override void Initialize()
        {
            base.Initialize();
           Screen.SetSize(1280, 800);
            DefaultSamplerState = SamplerState.LinearClamp;
            Scene = new BlankScene();
        }
    }
}