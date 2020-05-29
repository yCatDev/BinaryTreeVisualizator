using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;
using TinyAlgorithmVisualizer.Scenes;

namespace TinyAlgorithmVisualizer
{
    public class Menu: Scene
    {

        private GameUIHelper _uiHelper;
        private Table _table;
        private float _lastScroll;
        
        public override void Initialize()
        {
            base.Initialize();
            ClearColor = Color.Black;
            
            _uiHelper = new GameUIHelper(Content);
            var ui = CreateEntity("UI").AddComponent<UICanvas>();
            _table = ui.Stage.AddElement(new Table());
           
            
            _uiHelper.CreateTitleLabel(_table, "Tiny Algorithm Visualizer").SetFontScale(0.75f).SetAlignment(Align.Left);
            _table.Row();

            _uiHelper.CreateRegularLabel(_table, "Tree visualizer").SetAlignment(Align.Left);;
            _uiHelper.CreateVerticalIndent(_table, 200);
            _table.Row();

           
            _uiHelper.CreateBtn(_table, "Binary Tree", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new BinaryTreeScene())));
            _table.Row();
            
            _uiHelper.CreateBtn(_table, "AVL Tree", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new AvlTreeScene())));
            _table.Row();
            _table.Pack();
            
            
            _uiHelper.CreateRegularLabel(_table, "Greedy").SetAlignment(Align.Left);;
            _uiHelper.CreateVerticalIndent(_table, 200);
            _table.Row();

           
            _uiHelper.CreateBtn(_table, "Huffman", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new HuffmanScene())));
            _table.Row();



            _uiHelper.CreateRegularLabel(_table, "Data structures visualizer").SetAlignment(Align.Left);;
            _uiHelper.CreateVerticalIndent(_table, 200);
            _table.Row();

           
            _uiHelper.CreateBtn(_table, "Stack", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new StackScene())));
            _table.Row();
            
            _uiHelper.CreateBtn(_table, "Looped queue", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new QueueScene())));
            _table.Row();
            _table.Pack();
            
            _uiHelper.CreateBtn(_table, "Linked list", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new ListScene())));
            _table.Row();
            _table.Pack();
            
            
            _table.SetPosition(0, 0 );
        }

        public override void Update()
        {
            base.Update();
            var scroll = Mouse.GetState().ScrollWheelValue;
            if (Math.Abs(_lastScroll - scroll) > 0)
            {
                _table.SetY(_table.GetY() + -(_lastScroll - scroll)/2f);
                _lastScroll = scroll;
            }

        }
    }
}