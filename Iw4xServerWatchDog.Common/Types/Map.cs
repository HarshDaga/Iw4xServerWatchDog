using System;

namespace Iw4xServerWatchDog.Common.Types
{
	public class Map
	{
		public string Name { get; set; }
		public Uri Image { get; set; }

		public override string ToString ( ) => $"{Name} {Image.AbsoluteUri}";
	}
}