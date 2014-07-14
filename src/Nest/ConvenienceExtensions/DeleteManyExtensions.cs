﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Nest
{
	/// <summary>
	/// Provides GetMany extensions that make it easier to get many documents given a list of ids
	/// </summary>
	public static class DeleteManyExtensions
	{

		/// <summary>
		/// Shortcut into the <see cref="Bulk"/> call that deletes the specified objects
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="objects">List of objects to delete</param>
		/// <param name="index">Override the inferred indexname for T</param>
		/// <param name="type">Override the inferred typename for T</param>
		public static IBulkResponse DeleteMany<T>(this IElasticClient client, IEnumerable<T> @objects, string index = null, string type = null) where T : class
		{
			var bulkRequest = CreateDeleteBulkRequest(objects, index, type);
			return client.Bulk(bulkRequest);
		}
		

		/// <summary>
		/// Shortcut into the <see cref="Bulk"/> call that deletes the specified objects
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="objects">List of objects to delete</param>
		/// <param name="index">Override the inferred indexname for T</param>
		/// <param name="type">Override the inferred typename for T</param>
		public static Task<IBulkResponse> DeleteManyAsync<T>(this IElasticClient client, IEnumerable<T> objects, string index = null, string type = null)
			where T : class
		{
			var bulkRequest = CreateDeleteBulkRequest(objects, index, type);
			return client.BulkAsync(bulkRequest);
		}
		
		private static BulkRequest CreateDeleteBulkRequest<T>(IEnumerable<T> objects, string index, string type) where T : class
		{
			@objects.ThrowIfEmpty("objects");
			var bulkRequest = new BulkRequest {Index = index, Type = type};
			var deletes = @objects
				.Select(o => new BulkDeleteOperation<T>(o))
				.Cast<IBulkOperation>()
				.ToList();
			bulkRequest.Operations = deletes;
			return bulkRequest;
		}
	}
}