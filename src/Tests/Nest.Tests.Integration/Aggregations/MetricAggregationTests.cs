﻿using System.Linq;
using Elasticsearch.Net;
using FluentAssertions;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Integration.Aggregations
{
	[TestFixture]
	public class MetricAggregationTests : IntegrationTests
	{
		[Test]
		public void WrongFieldName()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.Min("value_agg", t => t
						.Field("this_field_name_does_not_exist")
					)
				)
			);
			results.IsValid.Should().BeTrue();
			var termBucket = results.Aggs.Min("value_agg");
			termBucket.Should().NotBeNull();
			termBucket.Value.Should().NotHaveValue();
		}

		[Test]
		public void Average()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.Average("value_agg", t => t.Field(p => p.LOC))
				)
			);
			results.IsValid.Should().BeTrue();
			var termBucket = results.Aggs.Min("value_agg");
			termBucket.Should().NotBeNull();
			termBucket.Value.Should().HaveValue();
		}
		[Test]
		public void Min()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.Min("value_agg", t => t.Field(p => p.LOC))
				)
			);
			results.IsValid.Should().BeTrue();
			var termBucket = results.Aggs.Min("value_agg");
			termBucket.Should().NotBeNull();
			termBucket.Value.Should().HaveValue();
		}
		
		[Test]
		[SkipVersion("0 - 1.0.9", "Cardinality aggregation not introduced until 1.1")]
		public void Cardinality()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.Cardinality("bucket_agg", m => m
						.Field(p => p.Country)
					)

				)
			);
			results.IsValid.Should().BeTrue();
			var metric = results.Aggs.Cardinality("bucket_agg");
			metric.Should().NotBeNull();
			metric.Value.Should().HaveValue();
			metric.Value.Value.Should().BeGreaterThan(0);

		}

		[Test]
		public void Max()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.Max("value_agg", t => t.Field(p => p.LOC))
				)
			);
			results.IsValid.Should().BeTrue();
			var termBucket = results.Aggs.Max("value_agg");
			termBucket.Should().NotBeNull();
			termBucket.Value.Should().HaveValue();
		}

		[Test]
		public void Sum()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.Sum("value_agg", t => t.Field(p => p.LOC))
				)
			);
			results.IsValid.Should().BeTrue();
			var termBucket = results.Aggs.Sum("value_agg");
			termBucket.Should().NotBeNull();
			termBucket.Value.Should().HaveValue();
		}

		[Test]
		public void ValueCount()
		{
			var results = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(a => a
					.ValueCount("value_agg", t => t.Field(p => p.LOC))
				)
			);
			results.IsValid.Should().BeTrue();
			var termBucket = results.Aggs.ValueCount("value_agg");
			termBucket.Should().NotBeNull();
			termBucket.Value.Should().HaveValue();
		}
	}
}