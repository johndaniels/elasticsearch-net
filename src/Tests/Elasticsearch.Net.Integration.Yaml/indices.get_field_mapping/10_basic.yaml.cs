using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Elasticsearch.Net.Integration.Yaml.IndicesGetFieldMapping1
{
	public partial class IndicesGetFieldMapping1YamlTests
	{	
	
		public class IndicesGetFieldMapping110BasicYamlBase : YamlTestsBase
		{
			public IndicesGetFieldMapping110BasicYamlBase() : base()
			{	

				//do indices.create 
				_body = new {
					mappings= new {
						test_type= new {
							properties= new {
								text= new {
									type= "string"
								}
							}
						}
					}
				};
				this.Do(()=> _client.IndicesCreate("test_index", _body));

				//do cluster.health 
				this.Do(()=> _client.ClusterHealth(nv=>nv
					.AddQueryString("wait_for_status", @"yellow")
				));

			}
		}


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class GetFieldMappingWithNoIndexAndType2Tests : IndicesGetFieldMapping110BasicYamlBase
		{
			[Test]
			public void GetFieldMappingWithNoIndexAndType2Test()
			{	

				//do indices.get_field_mapping 
				this.Do(()=> _client.IndicesGetFieldMappingForAll("text"));

				//match _response.test_index.mappings.test_type.text.mapping.text.type: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.type, @"string");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class GetFieldMappingByIndexOnly3Tests : IndicesGetFieldMapping110BasicYamlBase
		{
			[Test]
			public void GetFieldMappingByIndexOnly3Test()
			{	

				//do indices.get_field_mapping 
				this.Do(()=> _client.IndicesGetFieldMapping("test_index", "text"));

				//match _response.test_index.mappings.test_type.text.mapping.text.type: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.type, @"string");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class GetFieldMappingByTypeField4Tests : IndicesGetFieldMapping110BasicYamlBase
		{
			[Test]
			public void GetFieldMappingByTypeField4Test()
			{	

				//do indices.get_field_mapping 
				this.Do(()=> _client.IndicesGetFieldMapping("test_index", "test_type", "text"));

				//match _response.test_index.mappings.test_type.text.mapping.text.type: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.type, @"string");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class GetFieldMappingByTypeFieldWithAnotherFieldThatDoesntExist5Tests : IndicesGetFieldMapping110BasicYamlBase
		{
			[Test]
			public void GetFieldMappingByTypeFieldWithAnotherFieldThatDoesntExist5Test()
			{	

				//do indices.get_field_mapping 
				this.Do(()=> _client.IndicesGetFieldMapping("test_index", "test_type", "text,text1"));

				//match _response.test_index.mappings.test_type.text.mapping.text.type: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.type, @"string");

				//is_false _response.test_index.mappings.test_type.text1; 
				this.IsFalse(_response.test_index.mappings.test_type.text1);

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class GetFieldMappingWithIncludeDefaults6Tests : IndicesGetFieldMapping110BasicYamlBase
		{
			[Test]
			public void GetFieldMappingWithIncludeDefaults6Test()
			{	

				//do indices.get_field_mapping 
				this.Do(()=> _client.IndicesGetFieldMapping("test_index", "test_type", "text", nv=>nv
					.AddQueryString("include_defaults", @"true")
				));

				//match _response.test_index.mappings.test_type.text.mapping.text.type: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.type, @"string");

				//match _response.test_index.mappings.test_type.text.mapping.text.analyzer: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.analyzer, @"default");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class GetFieldMappingShouldWorkWithoutIndexSpecifyingTypeAndField7Tests : IndicesGetFieldMapping110BasicYamlBase
		{
			[Test]
			public void GetFieldMappingShouldWorkWithoutIndexSpecifyingTypeAndField7Test()
			{	

				//do indices.get_field_mapping 
				this.Do(()=> _client.IndicesGetFieldMappingForAll("test_type", "text"));

				//match _response.test_index.mappings.test_type.text.mapping.text.type: 
				this.IsMatch(_response.test_index.mappings.test_type.text.mapping.text.type, @"string");

			}
		}
	}
}

