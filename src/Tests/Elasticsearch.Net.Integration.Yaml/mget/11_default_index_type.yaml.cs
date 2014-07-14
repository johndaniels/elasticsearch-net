using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Elasticsearch.Net.Integration.Yaml.Mget2
{
	public partial class Mget2YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class DefaultIndexType1Tests : YamlTestsBase
		{
			[Test]
			public void DefaultIndexType1Test()
			{	

				//do indices.create 
				this.Do(()=> _client.IndicesCreate("test_2", null));

				//do index 
				_body = new {
					foo= "bar"
				};
				this.Do(()=> _client.Index("test_1", "test", "1", _body));

				//do cluster.health 
				this.Do(()=> _client.ClusterHealth(nv=>nv
					.AddQueryString("wait_for_status", @"yellow")
				));

				//do mget 
				_body = new {
					docs= new dynamic[] {
						new {
							_index= "test_2",
							_id= "1"
						},
						new {
							_type= "none",
							_id= "1"
						},
						new {
							_id= "2"
						},
						new {
							_id= "1"
						}
					}
				};
				this.Do(()=> _client.Mget("test_1", "test", _body));

				//is_false _response.docs[0].found; 
				this.IsFalse(_response.docs[0].found);

				//match _response.docs[0]._index: 
				this.IsMatch(_response.docs[0]._index, @"test_2");

				//match _response.docs[0]._type: 
				this.IsMatch(_response.docs[0]._type, @"test");

				//match _response.docs[0]._id: 
				this.IsMatch(_response.docs[0]._id, 1);

				//is_false _response.docs[1].found; 
				this.IsFalse(_response.docs[1].found);

				//match _response.docs[1]._index: 
				this.IsMatch(_response.docs[1]._index, @"test_1");

				//match _response.docs[1]._type: 
				this.IsMatch(_response.docs[1]._type, @"none");

				//match _response.docs[1]._id: 
				this.IsMatch(_response.docs[1]._id, 1);

				//is_false _response.docs[2].found; 
				this.IsFalse(_response.docs[2].found);

				//match _response.docs[2]._index: 
				this.IsMatch(_response.docs[2]._index, @"test_1");

				//match _response.docs[2]._type: 
				this.IsMatch(_response.docs[2]._type, @"test");

				//match _response.docs[2]._id: 
				this.IsMatch(_response.docs[2]._id, 2);

				//is_true _response.docs[3].found; 
				this.IsTrue(_response.docs[3].found);

				//match _response.docs[3]._index: 
				this.IsMatch(_response.docs[3]._index, @"test_1");

				//match _response.docs[3]._type: 
				this.IsMatch(_response.docs[3]._type, @"test");

				//match _response.docs[3]._id: 
				this.IsMatch(_response.docs[3]._id, 1);

				//match _response.docs[3]._version: 
				this.IsMatch(_response.docs[3]._version, 1);

				//match _response.docs[3]._source: 
				this.IsMatch(_response.docs[3]._source, new {
					foo= "bar"
				});

			}
		}
	}
}

