using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;

namespace Iw4xServerWatchDog.ProcessManagement
{
	public class ProcessWatcherCollection : IReadOnlyDictionary<int, ProcessWatcher>
	{
		private ImmutableDictionary<int, ProcessWatcher> watchers;

		public ProcessWatcherCollection ( )
		{
			watchers = ImmutableDictionary<int, ProcessWatcher>.Empty;
		}

		public void Add ( ProcessWatcher watcher )
		{
			Interlocked.Exchange ( ref watchers, watchers.SetItem ( watcher.Port, watcher ) );
		}

		public void Start ( int port )
		{
			if ( watchers.TryGetValue ( port, out var watcher ) )
				watcher.Start ( );
		}

		public void Kill ( int port )
		{
			if ( watchers.TryGetValue ( port, out var watcher ) )
				watcher.Kill ( );
		}

		public void Restart ( int port )
		{
			if ( watchers.TryGetValue ( port, out var watcher ) )
				watcher.Restart ( );
		}

		public void StartAll ( )
		{
			foreach ( var watcher in watchers.Values )
				watcher.Start ( );
		}

		public void KillAll ( )
		{
			foreach ( var watcher in watchers.Values )
				watcher.Kill ( );
		}

		public void RestartAll ( )
		{
			foreach ( var watcher in watchers.Values )
				watcher.Restart ( );
		}

		#region IReadOnlyDictionary Implementation

		public IEnumerator<KeyValuePair<int, ProcessWatcher>> GetEnumerator ( ) => watchers.GetEnumerator ( );

		IEnumerator IEnumerable.GetEnumerator ( ) => ( (IEnumerable) watchers ).GetEnumerator ( );

		public int Count => watchers.Count;

		public bool ContainsKey ( int key ) => watchers.ContainsKey ( key );

		public bool TryGetValue ( int key, out ProcessWatcher value ) => watchers.TryGetValue ( key, out value );

		public ProcessWatcher this [ int key ] => watchers[key];

		public IEnumerable<int> Keys => watchers.Keys;

		public IEnumerable<ProcessWatcher> Values => watchers.Values;

		#endregion
	}
}