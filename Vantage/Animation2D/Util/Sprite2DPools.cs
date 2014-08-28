// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sprite2DPools.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Vantage.Animation2D.Util
{
    using System;
    using System.Collections.Generic;

    public class Sprite2DPools
    {
        #region Fields

        private Dictionary<string, Sprite2DPool> pools = new Dictionary<string, Sprite2DPool>();

        private List<Sprite2DGroup> spriteGroups = new List<Sprite2DGroup>();

        private Storyboard storyboard;

        #endregion

        #region Constructors and Destructors

        public Sprite2DPools(Storyboard storyboard)
        {
            this.storyboard = storyboard;
        }

        #endregion

        #region Public Methods and Operators

        public void Clear()
        {
            foreach (var pool in this.pools)
            {
                pool.Value.Clear();
            }

            this.pools.Clear();
        }

        public Sprite2D Get(
            double startTime, 
            double endTime, 
            string path, 
            string layer, 
            string origin, 
            bool additive, 
            Sprite2DGroup spriteGroup, 
            int poolGroup)
        {
            return this.GetPool(path, layer, origin, additive, spriteGroup, poolGroup).Get(startTime, endTime);
        }

        public Sprite2D Get(
            double startTime, 
            double endTime, 
            string path, 
            string layer, 
            string origin, 
            bool additive, 
            Sprite2DGroup spriteGroup)
        {
            return this.GetPool(path, layer, origin, additive, spriteGroup, 0).Get(startTime, endTime);
        }

        public Sprite2D Get(
            double startTime, 
            double endTime, 
            string path, 
            string layer, 
            string origin, 
            bool additive, 
            int poolGroup)
        {
            return this.GetPool(path, layer, origin, additive, null, poolGroup).Get(startTime, endTime);
        }

        public Sprite2D Get(double startTime, double endTime, string path, string layer, string origin, bool additive)
        {
            return this.GetPool(path, layer, origin, additive, null, 0).Get(startTime, endTime);
        }

        public Sprite2D Get(double startTime, double endTime, string path, string layer, string origin, int poolGroup)
        {
            return this.GetPool(path, layer, origin, false, null, poolGroup).Get(startTime, endTime);
        }

        public Sprite2D Get(double startTime, double endTime, string path, string layer, string origin)
        {
            return this.GetPool(path, layer, origin, false, null, 0).Get(startTime, endTime);
        }

        public Sprite2D Get(double startTime, string path, string layer, string origin)
        {
            return this.GetPool(path, layer, origin, false, null, 0).Get(startTime);
        }

        public void Release(Sprite2D sprite, double endTime)
        {
            this.GetPool(sprite.ImageName, sprite.Layer, sprite.Origin, false, null, 0).Release(sprite, endTime);
        }

        #endregion

        #region Methods

        private string GetKey(
            string path, 
            string layer, 
            string origin, 
            bool additive, 
            Sprite2DGroup spriteGroup, 
            int poolGroup)
        {
            return path + "#" + layer + "#" + origin + "#" + (additive ? "1" : "0") + "#"
                   + this.GetSpriteGroupId(spriteGroup) + "#" + poolGroup;
        }

        private Sprite2DPool GetPool(
            string path, 
            string layer, 
            string origin, 
            bool additive, 
            Sprite2DGroup spriteGroup, 
            int poolGroup)
        {
            string key = this.GetKey(path, layer, origin, additive, spriteGroup, poolGroup);

            Sprite2DPool pool;
            if (!this.pools.TryGetValue(key, out pool))
            {
                pool = new Sprite2DPool(this.storyboard, path, layer, origin, additive, spriteGroup);
                this.pools.Add(key, pool);
            }

            return pool;
        }

        private int GetSpriteGroupId(Sprite2DGroup spriteGroup)
        {
            if (spriteGroup == null)
            {
                return -1;
            }

            var index = this.spriteGroups.IndexOf(spriteGroup);
            if (index < 0)
            {
                this.spriteGroups.Add(spriteGroup);
                return this.spriteGroups.Count - 1;
            }

            return index;
        }

        #endregion
    }
}