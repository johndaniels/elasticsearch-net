﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection;
using Elasticsearch.Net;

namespace Nest.Resolvers
{
	public class IndexNameResolver
	{
		private readonly IConnectionSettingsValues _connectionSettings;

		public IndexNameResolver(IConnectionSettingsValues connectionSettings)
		{
			connectionSettings.ThrowIfNull("connectionSettings");
			this._connectionSettings = connectionSettings;
		}

		public string GetIndexForType<T>()
		{
			return this.GetIndexForType(typeof(T));
		}

		public string GetIndexForType(Type type)
		{
			var defaultIndices = this._connectionSettings.DefaultIndices;

			if (defaultIndices == null)
				return this._connectionSettings.DefaultIndex;

			string value;
			if (defaultIndices.TryGetValue(type, out value) && !string.IsNullOrWhiteSpace(value))
				return value;
			return this._connectionSettings.DefaultIndex;
		}


		internal string GetIndexForType(IndexNameMarker i)
		{
			return i.Name ?? this.GetIndexForType(i.Type);
		}
	}
}
