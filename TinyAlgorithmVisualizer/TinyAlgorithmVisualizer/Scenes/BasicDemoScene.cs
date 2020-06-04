using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using TinyAlgorithmVisualizer.Algorithms.Tree;

namespace TinyAlgorithmVisualizer.Scenes
{
    public abstract class BasicDemoScene: Scene
    {
        protected GameUIHelper UiHelper;
        protected Entity Domain;

        public override void OnStart()
        {
            base.OnStart();
            
            AddRenderer(new ScreenSpaceRenderer(100, 9990));
            UiHelper = new GameUIHelper(this.Content);
            var ui = CreateEntity("UI").AddComponent<UICanvas>();
            ui.RenderLayer = 9990;
            var input = ui.Stage.AddElement(UiHelper.CreateInputField("Enter text", OnCommandEnter));
            

            Domain = CreateEntity("Domain");
            Domain.Position = new Vector2(Screen.Width/2f, Screen.Height/2f);
            Camera.Entity.AddComponent<ViewController>();
            ClearColor = Color.Black;
            AfterStartup();    
        }
        
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        protected virtual void AfterStartup()
        {
            
        }
        protected abstract void OnCommandEnter(TextField field);
    }
}