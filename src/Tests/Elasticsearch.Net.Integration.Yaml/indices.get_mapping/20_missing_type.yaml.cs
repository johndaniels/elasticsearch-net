using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Elasticsearch.Net.Integration.Yaml.IndicesGetMapping2
{
	public partial class IndicesGetMapping2YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class ReturnEmptyResponseWhenTypeDoesntExist1Tests : YamlTestsBase
		{
			[Test]
			public void ReturnEmptyResponseWhenTypeDoesntExist1Test()
			{	

				//do indices.create 
				_body = new {
					mappings= new {
						test_type= new {
							properties= new {
								text= new {
									type= "string",
									analyzer= "whitespace"
								}
							}
						}
					}
				};
				this.Do(()=> _client.IndicesCreate("test_index", _body));

				//do indices.get_mapping 
				this.Do(()=> _client.IndicesGetMapping("test_index", "not_test_type"));

				//match this._status: 
				this.IsMatch(this._status, new {});

			}
		}
	}
}

