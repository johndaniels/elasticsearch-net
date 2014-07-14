using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Elasticsearch.Net.Integration.Yaml.Create7
{
	public partial class Create7YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class Routing1Tests : YamlTestsBase
		{
			[Test]
			public void Routing1Test()
			{	

				//do indices.create 
				_body = new {
					settings= new {
						index= new {
							number_of_shards= "5",
							number_of_replicas= "0"
						}
					}
				};
				this.Do(()=> _client.IndicesCreate("test_1", _body));

				//do cluster.health 
				this.Do(()=> _client.ClusterHealth(nv=>nv
					.AddQueryString("wait_for_status", @"green")
				));

				//do create 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> _client.Index("test_1", "test", "1", _body, nv=>nv
					.AddQueryString("routing", 5)
					.AddQueryString("op_type", @"create")
				));

				//do get 
				this.Do(()=> _client.Get("test_1", "test", "1", nv=>nv
					.AddQueryString("routing", 5)
					.AddQueryString("fields", new [] {
						@"_routing"
					})
				));

				//match _response._id: 
				this.IsMatch(_response._id, 1);

				//match _response.fields._routing: 
				this.IsMatch(_response.fields._routing, 5);

				//do get 
				this.Do(()=> _client.Get("test_1", "test", "1"), shouldCatch: @"missing");

			}
		}
	}
}

