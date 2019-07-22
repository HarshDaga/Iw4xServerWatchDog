namespace Iw4xServerWatchDog.Common.Types
{
	public class Map
	{
		public string Name { get; set; }
		public string Image { get; set; }

		public override string ToString ( ) => $"{Name} {Image}";
	}
}