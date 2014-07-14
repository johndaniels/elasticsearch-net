﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Nest.Resolvers;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Nest.Resolvers.Converters;
using Elasticsearch.Net;

namespace Nest
{
	[JsonConverter(typeof(ReadAsTypeConverter<AndFilterDescriptor>))]
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IAndFilter : IFilter
	{
		[JsonProperty("filters", 
			ItemConverterType = typeof(CompositeJsonConverter<ReadAsTypeConverter<FilterContainer>, CustomJsonConverter>))]
		IEnumerable<IFilterContainer> Filters { get; set; }
	}
	
	public class AndFilter : PlainFilter, IAndFilter
	{
		protected internal override void WrapInContainer(IFilterContainer container)
		{
			container.And = this;
		}

		public IEnumerable<IFilterContainer> Filters { get; set; }
	}

	public class AndFilterDescriptor : FilterBase, IAndFilter
	{
		IEnumerable<IFilterContainer> IAndFilter.Filters { get; set; }

		bool IFilter.IsConditionless
		{
			get
			{
				var af = ((IAndFilter)this);
				return !af.Filters.HasAny() || af.Filters.All(f=>f.IsConditionless);
			} 
		}
	}
}
