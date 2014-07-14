using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nest.Resolvers;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeConverter<Ip4RangeAggregator>))]
	public interface IIp4RangeAggregator : IBucketAggregator
	{
		[JsonProperty("field")]
		PropertyPathMarker Field { get; set; }

		[JsonProperty(PropertyName = "ranges")]
		IEnumerable<Ip4Range> Ranges { get; set; }
	}

	public class Ip4RangeAggregator : BucketAggregator, IIp4RangeAggregator
	{
		public PropertyPathMarker Field { get; set; }
		public IEnumerable<Ip4Range> Ranges { get; set; }
	}

	public class Ip4RangeAggregationDescriptor<T> : BucketAggregationBaseDescriptor<Ip4RangeAggregationDescriptor<T>, T>, IIp4RangeAggregator 
		where T : class
	{

		public IIp4RangeAggregator Self { get { return this; } }

		PropertyPathMarker IIp4RangeAggregator.Field { get; set; }

		IEnumerable<Ip4Range> IIp4RangeAggregator.Ranges { get; set; }

		public Ip4RangeAggregationDescriptor<T> Field(string field)
		{
			Self.Field = field;
			return this;
		}

		public Ip4RangeAggregationDescriptor<T> Field(Expression<Func<T, object>> field)
		{
			Self.Field = field;
			return this;
		}

		public Ip4RangeAggregationDescriptor<T> Ranges(params string[] ranges)
		{
			var newRanges = from range in ranges let r = new Ip4Range().Mask(range) select r;
			Self.Ranges = newRanges;
			return this;
		}
	}
}