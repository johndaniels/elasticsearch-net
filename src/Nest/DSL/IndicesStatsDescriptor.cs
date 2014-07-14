﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IIndicesStatsRequest : IIndicesOptionalPath<IndicesStatsRequestParameters>
	{
		IEnumerable<TypeNameMarker> Types { get; set; }
		IEnumerable<IndicesStatsMetric> Metrics { get; set; }

	}

	internal static class IndicesStatsPathInfo
	{
		public static void Update(IConnectionSettingsValues settings, ElasticsearchPathInfo<IndicesStatsRequestParameters> pathInfo, IIndicesStatsRequest request)
		{
			if (request.Types.HasAny())
			{
				var inferrer = new ElasticInferrer(settings);
				var types = inferrer.TypeNames(request.Types);
				pathInfo.RequestParameters.AddQueryString("types", string.Join(",", types));
			}
			if (request.Metrics != null)
				pathInfo.Metric = request.Metrics.Cast<Enum>().GetStringValue();
			pathInfo.HttpMethod = PathInfoHttpMethod.GET;
		}
	}

	public partial class IndicesStatsRequest : IndicesOptionalPathBase<IndicesStatsRequestParameters>, IIndicesStatsRequest
	{
		public IEnumerable<IndicesStatsMetric> Metrics { get; set; }
		public IEnumerable<TypeNameMarker> Types { get; set; }

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<IndicesStatsRequestParameters> pathInfo)
		{
			IndicesStatsPathInfo.Update(settings, pathInfo , this);
		}

	}

	[DescriptorFor("IndicesStats")]
	public partial class IndicesStatsDescriptor : IndicesOptionalPathDescriptor<IndicesStatsDescriptor, IndicesStatsRequestParameters>, IIndicesStatsRequest
	{
		private IIndicesStatsRequest Self { get { return this; } }

		IEnumerable<TypeNameMarker> IIndicesStatsRequest.Types { get; set; }
		IEnumerable<IndicesStatsMetric> IIndicesStatsRequest.Metrics { get; set; }

		//<summary>A comma-separated list of fields for `completion` metric (supports wildcards)</summary>
		public IndicesStatsDescriptor Types(params TypeNameMarker[] completion_fields)
		{
			Self.RequestParameters.AddQueryString("types", completion_fields);
			return this;
		}

		public IndicesStatsDescriptor Metrics(params IndicesStatsMetric[] metrics)
		{
			Self.Metrics = metrics;
			return this;
		}

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<IndicesStatsRequestParameters> pathInfo)
		{
			IndicesStatsPathInfo.Update(settings, pathInfo, this);
		}
	}
}
