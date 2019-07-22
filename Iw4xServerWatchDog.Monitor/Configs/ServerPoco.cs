namespace Iw4xServerWatchDog.Monitor.Configs
{
	public class ServerPoco
	{
		public int Port { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public string ProcessName { get; set; } = "iw4x";
	}
}