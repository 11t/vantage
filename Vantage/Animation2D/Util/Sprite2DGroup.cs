namespace Vantage.Animation2D.Util
{
	using System.Collections.Generic;

	public class Sprite2DGroup 
	{
		private Storyboard storyboard;
		private List<Sprite2D> sprites = new List<Sprite2D>();

		public Sprite2DGroup(Storyboard storyboard) 
		{
			this.storyboard = storyboard;
		}

		public Sprite2D NewSprite2D(string path, string layer, string origin) 
		{
			Sprite2D sprite = this.storyboard.NewUnregisteredSprite2D(path, layer, origin);
			this.sprites.Add(sprite);
			return sprite;
		}

		public void Register() 
		{
			foreach (Sprite2D sprite in this.sprites)
				this.storyboard.RegisterSprite2D(sprite);
			this.sprites.Clear();
		}
	}
}
