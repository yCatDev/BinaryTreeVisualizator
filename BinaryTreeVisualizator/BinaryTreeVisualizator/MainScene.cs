using System;
using System.Collections;
using System.Threading;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace BinaryTreeVisualizator
{

    public class Helpers
    {
        public static IEnumerator WaitAndFreeze(Entity target)
        {
            yield return Coroutine.WaitForSeconds(0.1f);
            target.UpdateInterval = UInt32.MaxValue;
        }
    }

    public class MainScene : Scene
    {
        private GameUIHelper _uiHelper;
        
        public override void OnStart()
        {
            base.OnStart();
           
            var gridEntity  = CreateEntity("grid");
            gridEntity.AddComponent(new SpringGrid(new Rectangle((int)0, (int)0, (int) (Screen.Width), (int) (Screen.Height)), new Vector2(20))
            {
                GridMinorThickness = 0,
                GridMajorThickness = 4,
                GridMajorColor = Color.Gray
            });
            var grid = gridEntity.GetComponent<SpringGrid>();
            grid.RenderLayer = 9999;
            Core.StartCoroutine(Helpers.WaitAndFreeze(gridEntity));

            _uiHelper = new GameUIHelper(this.Content);
            var ui = CreateEntity("UI").AddComponent<UICanvas>();
            var mainTable = ui.Stage.AddElement(new Table());
            
            var input = _uiHelper.CreateInputField(mainTable, "Введіть команду");
            
            mainTable.Row();
            mainTable.Pack();

        }

        public override void Initialize()
        {
            base.Initialize();
            ClearColor = Color.Black;
        }
    }
}