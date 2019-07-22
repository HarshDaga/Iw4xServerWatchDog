using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PortableJsonConfig
{
	public interface IConfig
	{
		[JsonIgnore]
		string ConfigFileName { get; }

		[JsonIgnore]
		string ConfigFolderName { get; }

		bool TryValidate ( out IList<Exception> exceptions );
	}

	public interface IConfig<out TConfig> : IConfig
		where TConfig : IConfig<TConfig>
	{
		TConfig RestoreDefaults ( );
	}
}