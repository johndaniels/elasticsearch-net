﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Elasticsearch.Net;
using FluentAssertions;
using Nest;
using Nest.Resolvers;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Unit.ObjectInitializer.DeleteByQuery
{
	[TestFixture]
	public class DeleteByQueryTypedRequestUrlTests : BaseJsonTests
	{
		private readonly IElasticsearchResponse _status;

		[Test]
		public void Typed_Defaults()
		{
			var status = this._client.DeleteByQuery(new DeleteByQueryRequest<ElasticsearchProject>())
				.ConnectionStatus;

			status.RequestUrl.Should().EndWith("/nest_test_data/elasticsearchprojects/_query");
			status.RequestMethod.Should().Be("DELETE");
		}
	
		[Test]
		public void Typed_Overrides()
		{
			var status = this._client.DeleteByQuery(new DeleteByQueryRequest<ElasticsearchProject>
			{
				Indices = new IndexNameMarker[] { typeof(ElasticsearchProject), "my-other-index"},
				Types = new TypeNameMarker[] {"sometype"}
			}).ConnectionStatus;

			status.RequestUrl.Should().EndWith("/nest_test_data%2Cmy-other-index/sometype/_query");
			status.RequestMethod.Should().Be("DELETE");
		}
	
		[Test]
		public void Typed_AllIndices()
		{
			var status = this._client.DeleteByQuery(new DeleteByQueryRequest<ElasticsearchProject>
			{
				AllIndices = true
			})
				.ConnectionStatus;

			status.RequestUrl.Should().EndWith("/_all/elasticsearchprojects/_query");
			status.RequestMethod.Should().Be("DELETE");
		}
	
		[Test]
		public void Typed_AllTypes()
		{
			var status = this._client.DeleteByQuery(new DeleteByQueryRequest<ElasticsearchProject>
			{
				AllTypes = true
			})
				.ConnectionStatus;

			status.RequestUrl.Should().EndWith("/nest_test_data/_query");
			status.RequestMethod.Should().Be("DELETE");
		}
	
		[Test]
		public void Typed_AllIndicesAllTypes()
		{
			var status = this._client.DeleteByQuery(new DeleteByQueryRequest<ElasticsearchProject>
			{
				AllTypes = true,
				AllIndices = true
			})
				.ConnectionStatus;

			status.RequestUrl.Should().EndWith("/_all/_query");
			status.RequestMethod.Should().Be("DELETE");
		}
	
	}
}
