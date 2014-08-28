namespace Vantage.Animation2D.Util {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Sprite2DPools {
		private Storyboard storyboard;
		private Dictionary<String, Sprite2DPool> pools = new Dictionary<String, Sprite2DPool>();
		private List<Sprite2DGroup> spriteGroups = new List<Sprite2DGroup>();

		public Sprite2DPools(Storyboard storyboard) 
		{
			this.storyboard = storyboard;
		}

		public Sprite2D Get(double startTime, double endTime, String path, String layer, String origin, bool additive, Sprite2DGroup spriteGroup, int poolGroup) 
		{
			return this.GetPool(path, layer, origin, additive, spriteGroup, poolGroup).Get(startTime, endTime);
		}

		public Sprite2D Get(double startTime, double endTime, String path, String layer, String origin, bool additive, Sprite2DGroup spriteGroup) 
		{
			return this.GetPool(path, layer, origin, additive, spriteGroup, 0).Get(startTime, endTime);
		}

		public Sprite2D Get(double startTime, double endTime, String path, String layer, String origin, bool additive, int poolGroup) 
		{
			return this.GetPool(path, layer, origin, additive, null, poolGroup).Get(startTime, endTime);
		}

		public Sprite2D Get(double startTime, double endTime, String path, String layer, String origin, bool additive) 
		{
			return this.GetPool(path, layer, origin, additive, null, 0).Get(startTime, endTime);
		}

		public Sprite2D Get(double startTime, double endTime, String path, String layer, String origin, int poolGroup) 
		{
			return this.GetPool(path, layer, origin, false, null, poolGroup).Get(startTime, endTime);
		}

		public Sprite2D Get(double startTime, double endTime, String path, String layer, String origin) 
		{
			return this.GetPool(path, layer, origin, false, null, 0).Get(startTime, endTime);
		}

		public Sprite2D Get(double startTime, String path, String layer, String origin) 
		{
			return this.GetPool(path, layer, origin, false, null, 0).Get(startTime);
		}

		public void Release(Sprite2D sprite, double endTime) 
		{
			this.GetPool(sprite.ImageName, sprite.Layer, sprite.Origin, false, null, 0).Release(sprite, endTime);
		}

		public void Clear() 
		{
			foreach (var pool in this.pools)
				pool.Value.Clear();
			this.pools.Clear();
		}

		private Sprite2DPool GetPool(String path, String layer, String origin, bool additive, Sprite2DGroup spriteGroup, int poolGroup) 
		{
			String key = GetKey(path, layer, origin, additive, spriteGroup, poolGroup);

			Sprite2DPool pool;
			if (!this.pools.TryGetValue(key, out pool)) {
				pool = new Sprite2DPool(this.storyboard, path, layer, origin, additive, spriteGroup);
				pools.Add(key, pool);
			}

			return pool;
		}

		private String GetKey(String path, String layer, String origin, bool additive, Sprite2DGroup spriteGroup, int poolGroup) 
		{
			return path + "#" + layer + "#" + origin + "#" + (additive ? "1" : "0") + "#" + GetSpriteGroupId(spriteGroup) + "#" + poolGroup;
		}

		private int GetSpriteGroupId(Sprite2DGroup spriteGroup) 
		{
			if (spriteGroup == null)
				return -1;

			var index = spriteGroups.IndexOf(spriteGroup);
			if (index < 0) 
			{
				spriteGroups.Add(spriteGroup);
				return spriteGroups.Count - 1;
			}

			return index;
		}
	}
}
