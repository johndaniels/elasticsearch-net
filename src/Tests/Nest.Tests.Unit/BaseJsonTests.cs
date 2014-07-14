﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Nest;
using Newtonsoft.Json.Converters;
using Nest.Resolvers.Converters;
using Nest.Tests.MockData.Domain;
using System.Reflection;
using System.IO;
using Moq;
using PurifyNet;

namespace Nest.Tests.Unit
{
	public static class UnitTestDefaults
	{
		public static readonly Uri Uri = new Uri("http://localhost:9200");
		public static readonly string DefaultIndex = "nest_test_data";
	}

	public class BaseJsonTests
	{
		protected readonly IConnectionSettingsValues _settings;
		/// <summary>
		/// In memory client that never hits elasticsearch
		/// </summary>
		protected readonly IElasticClient _client;
		protected readonly IConnection _connection;

		public BaseJsonTests()
		{
			this._settings = new ConnectionSettings(UnitTestDefaults.Uri, UnitTestDefaults.DefaultIndex)
				.ExposeRawResponse();
			this._connection = new InMemoryConnection(this._settings);
			this._client = new ElasticClient(this._settings, this._connection);
		}

		protected void deb(string s)
		{
			//I use NCrunch for continuous testing
			//with this i can print variables as i type...
			//Lazy programmers for the win!
			throw new Exception(s);
		}
		protected ElasticClient GetFixedReturnClient(MethodBase methodInfo, string fileName)
		{
			var settings = new ConnectionSettings(UnitTestDefaults.Uri, UnitTestDefaults.DefaultIndex)
				.ExposeRawResponse();
			var file = this.GetFileFromMethod(methodInfo, fileName);
			var jsonResponse = File.ReadAllText(file);
			var connection = new InMemoryConnection(this._settings, jsonResponse);
			var client = new ElasticClient(settings, connection);
			return client;
		}
		protected void JsonEquals(object o, MethodBase method, string fileName = null)
		{
			if (o is byte[])
			{
				string s = ((byte[])o).Utf8String();
				this.JsonEquals(s, method, fileName);
				return;
			}
			var json = TestElasticClient.Serialize(o);
			this.JsonEquals(json, method, fileName);
		}

		protected void JsonEquals(string json, MethodBase method, string fileName = null)
		{
			var file = this.GetFileFromMethod(method, fileName);

			var expected = File.ReadAllText(file);
			//Assert.AreEqual(expected, json);
			Assert.True(json.JsonEquals(expected), this.PrettyPrint(json));
		}

		protected void JsonNotEquals(object o, MethodBase method, string fileName = null)
		{
			var json = TestElasticClient.Serialize(o);
			this.JsonNotEquals(json, method, fileName);
		}

		protected string ReadMethodJson(MethodBase method, string fileName = null)
		{
			var file = this.GetFileFromMethod(method, fileName);
			var expected = File.ReadAllText(file);
			return expected;
		}

		protected void JsonNotEquals(string json, MethodBase method, string fileName = null)
		{
			var file = this.GetFileFromMethod(method, fileName);
			var expected = File.ReadAllText(file);
			Assert.False(json.JsonEquals(expected), this.PrettyPrint(json));
		}

		protected void BulkJsonEquals(string json, MethodBase method, string fileName = null)
		{
			var file = this.GetFileFromMethod(method, fileName);
			var expected = File.ReadAllText(file);
			var expectedJsonLines = Regex.Split(expected, @"\r?\n", RegexOptions.None).Where(s => !s.IsNullOrEmpty());
			var jsonLines = Regex.Split(json, @"\r?\n", RegexOptions.None).Where(s => !s.IsNullOrEmpty());
			Assert.IsNotEmpty(expectedJsonLines, json);
			Assert.IsNotEmpty(jsonLines, json);
			Assert.AreEqual(jsonLines.Count(), expectedJsonLines.Count(), json);
			foreach (var line in expectedJsonLines.Zip(jsonLines, (e, j) => new { Expected = e, Json = j }))
			{
				Assert.True(line.Json.JsonEquals(line.Expected), line.Json);
			}
		}

		private string PrettyPrint(string json)
		{
			dynamic parsedJson = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}

		protected string GetFileFromMethod(MethodBase method, string fileName)
		{
			var type = method.DeclaringType;
			var @namespace = method.DeclaringType.Namespace;
			var folderSep = Path.DirectorySeparatorChar.ToString();
			var folder = @namespace.Replace("Nest.Tests.Unit.", "").Replace(".", folderSep);
			var file = Path.Combine(folder, (fileName ?? method.Name) + ".json");
			file = Path.Combine(Environment.CurrentDirectory.Replace("bin" + folderSep + "Debug", "").Replace("bin" + folderSep + "Release", ""), file);
			return file;
		}
	}
}
