namespace Vantage.Animation2D.Util
{
	using System;
	using System.Collections.Generic;

	public class Sprite2DPool 
	{
		private Storyboard storyboard;
		private string path;
		private string layer;
		private string origin;
		private bool additive;
		private Sprite2DGroup spriteGroup;

		private List<PooledSprite> pooledSprites = new List<PooledSprite>();

		public Sprite2DPool(Storyboard storyboard, String path, String layer, String origin, bool additive, Sprite2DGroup spriteGroup) 
		{
			this.storyboard = storyboard;
			this.path = path;
			this.layer = layer;
			this.origin = origin;
			this.additive = additive;
			this.spriteGroup = spriteGroup;
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
			foreach (var pooledSprite in pooledSprites) 
			{
				if (pooledSprite.endTime < startTime)
				{
					result = pooledSprite;
					break;
				}
			}

			if (result != null) 
			{
				pooledSprites.Remove(result);
				return result.sprite;
			}

			if (spriteGroup != null)
				return spriteGroup.NewSprite2D(path, layer, origin);

			return this.storyboard.NewSprite2D(path, layer, origin);
		}

		public void Release(Sprite2D sprite, double endTime) 
		{
			this.pooledSprites.Add(new PooledSprite(sprite, endTime));
		}

		public void Clear() {
			if (additive) {
				foreach (PooledSprite pooledSprite in this.pooledSprites) {
					var sprite = pooledSprite.sprite;
					sprite.AdditiveP(sprite.GetCommandsStartTime(), (int)pooledSprite.endTime);
				}
			}

			this.pooledSprites.Clear();
		}

		class PooledSprite 
		{
			public Sprite2D sprite;
			public double endTime;

			public PooledSprite(Sprite2D sprite, double endTime) 
			{
				this.sprite = sprite;
				this.endTime = endTime;
			}
		}
	}
}
