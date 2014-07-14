using System.Collections.Generic;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
using Nest.Resolvers;

namespace Nest
{
	[JsonObject(MemberSerialization.OptIn)]
	public class NumberMapping : IElasticType, IElasticCoreType
	{
		public PropertyNameMarker Name { get; set; }

		private TypeNameMarker __type;
		[JsonProperty("type")]
		public virtual TypeNameMarker Type { get { return (TypeNameMarker)(__type ?? "double"); } set { __type = value; } }

		[JsonProperty("similarity")]
		public string Similarity { get; set; }

		/// <summary>
		/// The name of the field that will be stored in the index. Defaults to the property/field name.
		/// </summary>
		[JsonProperty("index_name")]
		public string IndexName { get; set; }

		[JsonProperty("store")]
		public bool? Store { get; set; }

		[JsonProperty("index"), JsonConverter(typeof(StringEnumConverter))]
		public NonStringIndexOption? Index { get; set; }

		[JsonProperty("precision_step")]
		public int? PrecisionStep { get; set; }

		[JsonProperty("boost")]
		public double? Boost { get; set; }

		[JsonProperty("null_value")]
		public double? NullValue { get; set; }

		[JsonProperty("doc_values")]
		public bool? DocValues { get; set; }

		[JsonProperty("include_in_all")]
		public bool? IncludeInAll { get; set; }

		[JsonProperty("ignore_malformed")]
		public bool? IgnoreMalformed { get; set; }

		[JsonProperty("coerce")]
		public bool? Coerce { get; set; }

	}
}