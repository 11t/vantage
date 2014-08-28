// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sprite2DGroup.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Vantage.Animation2D.Util
{
    using System.Collections.Generic;

    public class Sprite2DGroup
    {
        #region Fields

        private List<Sprite2D> sprites = new List<Sprite2D>();

        private Storyboard storyboard;

        #endregion

        #region Constructors and Destructors

        public Sprite2DGroup(Storyboard storyboard)
        {
            this.storyboard = storyboard;
        }

        #endregion

        #region Public Methods and Operators

        public void InsertSprites()
        {
            foreach (Sprite2D sprite in this.sprites)
            {
                this.storyboard.RegisterSprite2D(sprite);
            }

            this.sprites.Clear();
        }

        public Sprite2D NewSprite2D(string path, string layer, string origin)
        {
            Sprite2D sprite = this.storyboard.NewUnregisteredSprite2D(path, layer, origin);
            this.sprites.Add(sprite);
            return sprite;
        }

        #endregion
    }
}