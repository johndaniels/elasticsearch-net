﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Elasticsearch.Net;
using FluentAssertions;
using Nest;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Unit.ObjectInitializer.MultiTermVectors
{
	[TestFixture]
	public class MultiTermVectorsRequestTests : BaseJsonTests
	{
		private readonly IElasticsearchResponse _status;

		public MultiTermVectorsRequestTests()
		{
			var documents = Enumerable.Range(1,4).Select(i=>new MultiTermVectorDocument { Id = i.ToString(CultureInfo.InvariantCulture), FieldStatistics = true });
			var request = new MultiTermVectorsRequest("my-index","zhe-type")
			{
				Documents = documents,
				FieldStatistics = false,
				Payloads = true,
				Positions = false
			};
			var response = this._client.MultiTermVectors(request);
			this._status = response.ConnectionStatus;
		}

		[Test]
		public void Url()
		{
			this._status.RequestUrl.Should().EndWith("/_mtermvectors?field_statistics=false&payloads=true&positions=false");
			this._status.RequestMethod.Should().Be("POST");
		}
		
		[Test]
		public void MultiTermVectorsBody()
		{
			this.JsonEquals(this._status.Request.Utf8String(), MethodBase.GetCurrentMethod());
		}
	}
}
