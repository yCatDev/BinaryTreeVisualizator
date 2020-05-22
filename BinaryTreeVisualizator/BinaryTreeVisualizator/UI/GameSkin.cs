﻿using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using Nez.Systems;
using Nez.UI;

namespace BinaryTreeVisualizator
{
    
    /// <summary>
    /// Class that creates base skin style for game ui
    /// </summary>
    public class GameSkin
    {
        /// <summary>
        /// Nez skin
        /// </summary>
        public readonly Skin Skin;

        /// <summary>
        /// Creates skin styles
        /// </summary>
        /// <param name="contentManager">Content manager</param>
        public GameSkin(NezContentManager contentManager)
        {
            Skin = new Skin();

            Skin.Add("title-label", new LabelStyle()
            {
              Font = contentManager.LoadBitmapFont(Content.OswaldTitleFont)
            });

            Skin.Add("label", new LabelStyle()
            {
                Font = contentManager.LoadBitmapFont(Content.DefaultTitleFont)
            });

            var inputCursor = new PrimitiveDrawable(Color.Black);
            inputCursor.MinHeight = 10;
            inputCursor.MinWidth = 5;
            var font = contentManager.LoadBitmapFont(Content.DefaultTitleFont);
            font.FontSize = 24;
            var style = Skin.Add("inputfield", new TextFieldStyle()
            {
                Font = font,
                FontColor = Color.Black,
                Cursor = inputCursor,
                FocusedBackground = new PrimitiveDrawable(Color.Gray),
                Background = new PrimitiveDrawable(Color.White),
                Selection = new PrimitiveDrawable(Color.Blue)
            });
            
            Skin.Add("regular-button", TextButtonStyle.Create(Color.Gray, new Color(61, 9, 85), new Color(61, 9, 107)));
            
            
            var sliderStyle = SliderStyle.Create(Color.Yellow, new Color(61, 9, 107));
            
            sliderStyle.Knob.MinWidth *= 1.5f;
            sliderStyle.Knob.MinHeight *= 1.5f;
            sliderStyle.Background.MinWidth *= 0.5f;

            Skin.Add("slider", sliderStyle);

        }
    }
}