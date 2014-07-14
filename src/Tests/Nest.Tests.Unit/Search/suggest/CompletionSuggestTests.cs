﻿using Elasticsearch.Net;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest.Tests.Unit.Search.Suggest
{
	[TestFixture]
	public class CompletionSuggestTests : BaseJsonTests
	{
		[Test]
		public void CompletionSuggestDescriptorTest()
		{
			var completionSuggestDescriptor = new CompletionSuggestDescriptor<ElasticsearchProject>()
				.OnField("suggest")
				.Text("n");

			var json = TestElasticClient.Serialize(completionSuggestDescriptor);

			var expected = @"{ field: ""suggest"" }";

			Assert.IsTrue(json.JsonEquals(expected), json);
		}

		[Test]
		public void CompletionSuggestDescriptorDefaultFuzzinessTest()
		{
			var completionSuggestDescriptor = new CompletionSuggestDescriptor<ElasticsearchProject>()
				.OnField("suggest")
				.Text("n")
				.Fuzzy();

			var json = TestElasticClient.Serialize(completionSuggestDescriptor);

			var expected = @"{
                              field: ""suggest"",
                              fuzzy: {},
                            }";

			Assert.IsTrue(json.JsonEquals(expected), json);
		}

		[Test]
		public void CompletionSuggestDescriptorFuzzyTest()
		{
			var completionSuggestDescriptor = new CompletionSuggestDescriptor<ElasticsearchProject>()
				.OnField("suggest")
				.Text("n")
				.Fuzzy(f => f
					.Transpositions(false)
					.MinLength(5)
					.PrefixLength(4));

			var json = TestElasticClient.Serialize(completionSuggestDescriptor);

			var expected = @"{
                              field: ""suggest"",
                              ""fuzzy"": {
                                transpositions: false,
                                min_length: 5,
                                prefix_length: 4
                              }
                            }";

			Assert.IsTrue(json.JsonEquals(expected), json);
		}

		[Test]
		public void CompletionSuggestDescriptorFuzzinessTest()
		{
			var completionSuggestDescriptor = new CompletionSuggestDescriptor<ElasticsearchProject>()
				.OnField("suggest")
				.Text("n")
				.Fuzzy(f => f.Fuzziness(1));

			var json = TestElasticClient.Serialize(completionSuggestDescriptor);

			var expected = @"{
                              field: ""suggest"",
                              fuzzy: {
                                fuzziness: 1
                              }
                            }";

			Assert.IsTrue(json.JsonEquals(expected), json);
		}

		[Test]
		public void CompletionSuggestDescriptorFuzzinessDoubleTest()
		{
			var completionSuggestDescriptor = new CompletionSuggestDescriptor<ElasticsearchProject>()
				.OnField("suggest")
				.Text("n")
				.Fuzzy(f => f.Fuzziness(0.4));

			var json = TestElasticClient.Serialize(completionSuggestDescriptor);

			var expected = @"{
                              field: ""suggest"",
                              fuzzy: {
                                fuzziness: 0.4
                              }
                            }";

			Assert.IsTrue(json.JsonEquals(expected), json);
		}

		[Test]
		public void CompletionSuggestOnSearchTest()
		{
			var search = this._client.Search<ElasticsearchProject>(s => s
				.SuggestCompletion("mycompletionsuggest", ts => ts
					.Text("n")
					.OnField(p => p.Name)
					.Fuzzy()
				)
			);

			var expected = @"{
				suggest: {
					mycompletionsuggest: {
						text: ""n"",
						completion: {
							field: ""name"",
							fuzzy: {}
						}
					}
				}
			}";
			var json = search.ConnectionStatus.Request.Utf8String();
			Assert.True(json.JsonEquals(expected), json);
		}

		[Test]
		public void CompletionSuggestWithFuzzinessOnSearchTest()
		{
			var search = this._client.Search<ElasticsearchProject>(s => s
				.SuggestCompletion("mycompletionsuggest", ts => ts
					.Text("n")
					.OnField(p => p.Name)
					.Fuzzy()
				)
			);

			var expected = @"{
				suggest: {
					mycompletionsuggest: {
						text: ""n"",
						completion: {
							field: ""name"",
							fuzzy: {},
						}
					}
				}
			}";
			var json = search.ConnectionStatus.Request.Utf8String();
			Assert.True(json.JsonEquals(expected), json);
		}
	}
}
