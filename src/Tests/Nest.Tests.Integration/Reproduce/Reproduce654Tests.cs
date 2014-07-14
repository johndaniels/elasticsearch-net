﻿using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Elasticsearch.Net.Exceptions;
using Nest.Tests.MockData;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;
using System.Diagnostics;
using FluentAssertions;

namespace Nest.Tests.Integration.Reproduce
{
	/// <summary>
	/// https://github.com/Mpdreamz/NEST/issues/654
	/// </summary>
	[TestFixture]
	public class Reproduce654Tests : IntegrationTests
	{
		
		// a 404 on delete is a valid http status so IsValid should be true
		
		[Test]
		public void NestDeleteAsync_ShouldNotThrowOn404()
		{
			Assert.DoesNotThrow(async () =>
			{
				var deleteResult = await this._client.DeleteAsync("index", "type", "id");
				deleteResult.IsValid.Should().BeTrue();
				var e = deleteResult.ConnectionStatus.OriginalException as ElasticsearchServerException;
				e.Should().BeNull();
				deleteResult.Found.Should().BeFalse();
				deleteResult.ConnectionStatus.HttpStatusCode.Should().Be(404);
			});
		}
		[Test]
		public void NestDelete_ShouldNotThrowOn404()
		{
			Assert.DoesNotThrow(() =>
			{
				var deleteResult = this._client.Delete("index", "type", "id");
				deleteResult.IsValid.Should().BeTrue();
				var e = deleteResult.ConnectionStatus.OriginalException as ElasticsearchServerException;
				e.Should().BeNull();
				deleteResult.Found.Should().BeFalse();
				deleteResult.ConnectionStatus.HttpStatusCode.Should().Be(404);
			});
		}

		// clientThatThrows is a client configured to throw on wrong http status codes
		// 404 is a valid statis code for delete so lets test that no exceptions are thrown

		[Test]
		public void NestDeleteAsync_ShouldNotThrowOn404_OnClienThatThrows()
		{
			Assert.DoesNotThrow(async () =>
			{
				var deleteResult = await this._clientThatThrows.DeleteAsync("index", "type", "id");
				deleteResult.IsValid.Should().BeTrue();
				var e = deleteResult.ConnectionStatus.OriginalException as ElasticsearchServerException;
				e.Should().BeNull();
				deleteResult.Found.Should().BeFalse();
				deleteResult.ConnectionStatus.HttpStatusCode.Should().Be(404);
			});
		}
		[Test]
		public void NestDelete_ShouldNotThrowOn404_OnClientThatThrows()
		{
			Assert.DoesNotThrow(() =>
			{
				var deleteResult = this._clientThatThrows.Delete("index", "type", "id");
				deleteResult.IsValid.Should().BeTrue();
				var e = deleteResult.ConnectionStatus.OriginalException as ElasticsearchServerException;
				e.Should().BeNull();
				deleteResult.Found.Should().BeFalse();
				deleteResult.ConnectionStatus.HttpStatusCode.Should().Be(404);
			});
		}
	
		// The knowledge that 404 is valid on delete should live in Elasticsearch.NET
		// So lets double check that calling the low level client behaves the same as NEST

		[Test]
		public async void LowLevelClientDeleteAsync_ShouldNotThrowOn404()
		{
			Assert.DoesNotThrow(async () =>
			{
				var deleteResult = await this._client.Raw.DeleteAsync("index", "type", "id");
				deleteResult.Success.Should().BeTrue();
				var e = deleteResult.OriginalException as ElasticsearchServerException;
				deleteResult.HttpStatusCode.Should().Be(404);
				e.Should().BeNull();
				dynamic d = deleteResult.Response;
				bool found = d.found;
				found.Should().BeFalse();
			});
		}
		[Test]
		public void LowLevelClientDelete_ShouldNotThrowOn404()
		{
			Assert.DoesNotThrow(() =>
			{
				var deleteResult = this._client.Raw.Delete("index", "type", "id");
				deleteResult.Success.Should().BeTrue();
				var e = deleteResult.OriginalException as ElasticsearchServerException;
				deleteResult.HttpStatusCode.Should().Be(404);
				e.Should().BeNull();
				dynamic d = deleteResult.Response;
				bool found = d.found;
				found.Should().BeFalse();
			});
		}
		
		[Test]
		public async void LowLevelClientDeleteAsync_ShouldNotThrowOn404_OnClientThatThrows()
		{
			Assert.DoesNotThrow(async () =>
			{
				var deleteResult = await this._clientThatThrows.Raw.DeleteAsync("index", "type", "id");
				deleteResult.Success.Should().BeTrue();
				var e = deleteResult.OriginalException as ElasticsearchServerException;
				deleteResult.HttpStatusCode.Should().Be(404);
				e.Should().BeNull();
				dynamic d = deleteResult.Response;
				bool found = d.found;
				found.Should().BeFalse();
			});
		}

		[Test]
		public void LowLevelClientDelete_ShouldNotThrowOn404_OnClientThatThrows()
		{
			Assert.DoesNotThrow(() =>
			{
				var deleteResult = this._clientThatThrows.Raw.Delete("index", "type", "id");
				deleteResult.Success.Should().BeTrue();
				var e = deleteResult.OriginalException as ElasticsearchServerException;
				deleteResult.HttpStatusCode.Should().Be(404);
				e.Should().BeNull();
				dynamic d = deleteResult.Response;
				bool found = d.found;
				found.Should().BeFalse();
			});
		}

		// issueing a malformed _search causes a 400 on elasticsearch this should cause .IsValid
		// unless ThrowOnElasticsearchServerExceptions is set on the connection settings.

		[Test]
		public void NestSearchAsync_ShouldNotThrowOn400()
		{
			Assert.DoesNotThrow(async () =>
			{
				var searchResult = await this._client.SearchAsync<dynamic>(s=>s.QueryRaw("{ blah: blah }"));
				searchResult.IsValid.Should().BeFalse();
				searchResult.ConnectionStatus.HttpStatusCode.Should().Be(400);
				var e = searchResult.ConnectionStatus.OriginalException as ElasticsearchServerException;
				e.Should().NotBeNull();
				e.ExceptionType.Should().Be("SearchPhaseExecutionException");
				e.Message.Should().Contain("Unrecognized token 'blah'");
			});
		}
		[Test]
		public void NestSearch_ShouldNotThrowOn400()
		{
			Assert.DoesNotThrow(() =>
			{	
				var searchResult = this._client.Search<dynamic>(s=>s.QueryRaw("{ blah: blah }"));
				searchResult.IsValid.Should().BeFalse();
				searchResult.ConnectionStatus.HttpStatusCode.Should().Be(400);
				var e = searchResult.ConnectionStatus.OriginalException as ElasticsearchServerException;
				e.Should().NotBeNull();
				e.ExceptionType.Should().Be("SearchPhaseExecutionException");
				e.Message.Should().Contain("Unrecognized token 'blah'");
			});
		}

		//on a client that throws we should see an exception.
		[Test]
		public void NestSearchAsync_ShouldThrowOn400_OnClientThatThrows()
		{
			var e = Assert.Throws<ElasticsearchServerException>(async () =>
			{
				var searchResult = await this._clientThatThrows.SearchAsync<dynamic>(s=>s.QueryRaw("{ blah: blah }"));
			});
			e.Should().NotBeNull();
			e.ExceptionType.Should().Be("SearchPhaseExecutionException");
			e.Message.Should().Contain("Unrecognized token 'blah'");
		}
		[Test]
		public void NestSearch_ShouldThrowOn400_OnClientThatThrows()
		{
			var e = Assert.Throws<ElasticsearchServerException>(() =>
			{	
				var searchResult = this._clientThatThrows.Search<dynamic>(s=>s.QueryRaw("{ blah: blah }"));
				searchResult.IsValid.Should().BeFalse();
				searchResult.ConnectionStatus.HttpStatusCode.Should().Be(400);
			});
			e.Should().NotBeNull();
			e.ExceptionType.Should().Be("SearchPhaseExecutionException");
			e.Message.Should().Contain("Unrecognized token 'blah'");
		}
		
		// issueing a malformed _search causes a 400 on elasticsearch this should cause .Success
		// being false in Elasticsearch.Net
		// unless ThrowOnElasticsearchServerExceptions is set on the connection settings.

		[Test]
		public void LowLevelClientSearchAsync_ShouldNotThrowOn400()
		{
			Assert.DoesNotThrow(async () =>
			{
				var searchResult = await this._client.Raw.SearchAsync("{ blah: blah }");
				searchResult.Success.Should().BeFalse();
				searchResult.HttpStatusCode.Should().Be(400);
				var e = searchResult.OriginalException as ElasticsearchServerException;
				e.Should().NotBeNull();
				e.ExceptionType.Should().Be("SearchPhaseExecutionException");
				e.Message.Should().Contain("Unrecognized token 'blah'");
			});
		}
		[Test]
		public void LowLevelClientSearch_ShouldNotThrowOn400()
		{
			Assert.DoesNotThrow(() =>
			{	
				var searchResult = this._client.Raw.Search("{ blah: blah }");
				searchResult.Success.Should().BeFalse();
				searchResult.HttpStatusCode.Should().Be(400);
				var e = searchResult.OriginalException as ElasticsearchServerException;
				e.Should().NotBeNull();
				e.ExceptionType.Should().Be("SearchPhaseExecutionException");
				e.Message.Should().Contain("Unrecognized token 'blah'");
			});
		}

		//on a low level client that throws we should see an exception.
		[Test]
		public void LowLevelClientSearchAsync_ShouldThrowOn400_OnClientThatThrows()
		{
			var e = Assert.Throws<ElasticsearchServerException>(async () =>
			{
				var searchResult = await this._clientThatThrows.Raw.SearchAsync("{ blah: blah }");
			});
			e.Should().NotBeNull();
			e.ExceptionType.Should().Be("SearchPhaseExecutionException");
			e.Message.Should().Contain("Unrecognized token 'blah'");
		}
		[Test]
		public void LowLevelClientSearch_ShouldThrowOn400_OnClientThatThrows()
		{
			var e = Assert.Throws<ElasticsearchServerException>(() =>
			{	
				var searchResult = this._clientThatThrows.Raw.Search("{ blah: blah }");
			});
			e.Should().NotBeNull();
			e.ExceptionType.Should().Be("SearchPhaseExecutionException");
			e.Message.Should().Contain("Unrecognized token 'blah'");
		}
		
		[Test]
		public async void LowLevelClientSearchAsync_ShouldThrowOn400_OnClientThatThrows_TryCatch()
		{
			var seenError = false;
			try
			{
				var searchResult = await this._clientThatThrows.Raw.SearchAsync("{ blah: blah }");
			}
			catch (ElasticsearchServerException e)
			{
				seenError = true;
				e.Should().NotBeNull();
				e.ExceptionType.Should().Be("SearchPhaseExecutionException");
				e.Message.Should().Contain("Unrecognized token 'blah'");
			}
			catch (Exception e)
			{
				seenError = true;
				Assert.Fail("Did not expect to see exception of type {0}", e.GetType().FullName);
			}
			finally
			{
				if (!seenError)
					Assert.Fail("expected SearchAsync to thorw ElasticsearchServerException but did not receive ANY exception");
			}
		}
	}
}
