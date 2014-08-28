// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sprite2DPool.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Vantage.Animation2D.Util
{
    using System;
    using System.Collections.Generic;

    public class Sprite2DPool
    {
        #region Fields

        private bool additive;

        private string layer;

        private string origin;

        private string path;

        private List<PooledSprite> pooledSprites = new List<PooledSprite>();

        private Sprite2DGroup spriteGroup;

        private Storyboard storyboard;

        #endregion

        #region Constructors and Destructors

        public Sprite2DPool(
            Storyboard storyboard, 
            string path, 
            string layer, 
            string origin, 
            bool additive, 
            Sprite2DGroup spriteGroup)
        {
            this.storyboard = storyboard;
            this.path = path;
            this.layer = layer;
            this.origin = origin;
            this.additive = additive;
            this.spriteGroup = spriteGroup;
        }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            if (this.additive)
            {
                foreach (PooledSprite pooledSprite in this.pooledSprites)
                {
                    var sprite = pooledSprite.Sprite;
                    sprite.AdditiveP(sprite.GetCommandsStartTime(), (int)pooledSprite.EndTime);
                }
            }

            this.pooledSprites.Clear();
        }

        public Sprite2D Get(double startTime, double endTime)
        {
            Sprite2D sprite = this.Get(startTime);
            this.Release(sprite, endTime);
            return sprite;
        }

        public Sprite2D Get(double startTime)
        {
            var result = (PooledSprite)null;
            foreach (var pooledSprite in this.pooledSprites)
            {
                if (pooledSprite.EndTime < startTime)
                {
                    result = pooledSprite;
                    break;
                }
            }

            if (result != null)
            {
                this.pooledSprites.Remove(result);
                return result.Sprite;
            }

            if (this.spriteGroup != null)
            {
                return this.spriteGroup.NewSprite2D(this.path, this.layer, this.origin);
            }

            return this.storyboard.NewSprite2D(this.path, this.layer, this.origin);
        }

        public void Release(Sprite2D sprite, double endTime)
        {
            this.pooledSprites.Add(new PooledSprite(sprite, endTime));
        }

        #endregion

        private class PooledSprite
        {
            public PooledSprite(Sprite2D sprite, double endTime)
            {
                this.Sprite = sprite;
                this.EndTime = endTime;
            }

            public double EndTime { get; set; }

            public Sprite2D Sprite { get; set; }
        }
    }
}