using System.Collections.Generic;
using System.Drawing;
using Iw4xServerWatchDog.Common.Types;

namespace Iw4xServerWatchDog.Common.Configs
{
	public interface ICommonResources
	{
		Dictionary<string, string> Color { get; set; }
		Dictionary<string, Color> DrawingColor { get; set; }
		Dictionary<string, string> GameType { get; set; }
		Dictionary<string, Map> Map { get; set; }
	}
}