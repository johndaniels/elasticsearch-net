﻿using Elasticsearch.Net;
using NUnit.Framework;

namespace Nest.Tests.Integration.Cluster
{
	[TestFixture]
	public class GetSettingsTests : IntegrationTests
	{
		[Test]
		public void GetSettings()
		{
			var putSettingsResponse = this._client.ClusterSettings(p=>p
				.FlatSettings()
				.Transient(t=>t
					.Add("discovery.zen.minimum_master_nodes", 1)
				)
			);
			
			var r = this._client.ClusterGetSettings(p=>p
				.FlatSettings()
			);
			Assert.True(r.IsValid);
			Assert.AreEqual(r.Transient["discovery.zen.minimum_master_nodes"], "1");
		}
		
	}
}