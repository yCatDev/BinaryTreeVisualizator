using System;
using BinaryTreeVisualizator.Tree;
using Microsoft.Xna.Framework;
using Nez;

namespace BinaryTreeVisualizator
{
    /// <summary>
    /// Empty scene for creating animated transition of other scenes
    /// </summary>
    public class BlankScene: Scene
    {
        public override void OnStart()
        {
            base.OnStart();
            ClearColor = Color.Black;
            //Load menu scene
            var tree = new BinaryTree<int>()
            {
                4,3,0,1,12,5,6,7,8
            };

            //tree.Print(tree.GetNode(),10,0);
            //tree.Print(tree.GetNode(), 10,10);
           

            Core.StartSceneTransition(new FadeTransition(() => new MainScene()));
        }
        
        
    }
}