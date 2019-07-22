using System.Collections.Generic;

namespace Iw4xServerWatchDog.ProcessManagement
{
	public interface IProcessWatcherService : IReadOnlyDictionary<int, ProcessWatcher>
	{
		void Add ( ProcessWatcher watcher );
		void Kill ( int port );
		void KillAll ( );
		void Restart ( int port );
		void RestartAll ( );
		void Start ( int port );
		void StartAll ( );
	}
}