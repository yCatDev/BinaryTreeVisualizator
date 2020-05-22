using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace BinaryTreeVisualizator
{
    public class Menu: Scene
    {

        private GameUIHelper _uiHelper;
        
        public override void Initialize()
        {
            base.Initialize();
            ClearColor = Color.Black;
            
            _uiHelper = new GameUIHelper(Content);
            var ui = CreateEntity("UI").AddComponent<UICanvas>();
            var table = ui.Stage.AddElement(new Table());
           
            
            _uiHelper.CreateTitleLabel(table, "Binary Tree Visualizer").SetFontScale(0.75f).SetAlignment(Align.Left);
            table.Row();

            _uiHelper.CreateRegularLabel(table, "Select tree type").SetAlignment(Align.Left);;
            _uiHelper.CreateVerticalIndent(table, 200);
            table.Row();

           
            _uiHelper.CreateBtn(table, "Binary Tree", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new BinaryTreeScene())));
            table.Row();
            
            _uiHelper.CreateBtn(table, "AVL Tree", (btn) =>
                Core.StartSceneTransition(new FadeTransition(() => new AVLTreeScene())));
            table.Row();
            table.Pack();
            
            table.SetPosition(0, 0 );
        }
    }
}