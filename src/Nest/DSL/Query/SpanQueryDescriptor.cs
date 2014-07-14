﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeConverter<SpanQuery<object>>))]
	public interface ISpanQuery : IQuery
	{
		[JsonProperty(PropertyName = "span_term")]
		ISpanTermQuery SpanTermQueryDescriptor { get; set; }

		[JsonProperty(PropertyName = "span_first")]
		ISpanFirstQuery SpanFirst { get; set; }

		[JsonProperty(PropertyName = "span_near")]
		ISpanNearQuery SpanNear { get; set; }

		[JsonProperty(PropertyName = "span_or")]
		ISpanOrQuery SpanOr { get; set; }

		[JsonProperty(PropertyName = "span_not")]
		ISpanNotQuery SpanNot { get; set; }
	}

	public class SpanQuery : ISpanQuery
	{
		bool IQuery.IsConditionless { get { return false; } }
		public ISpanTermQuery SpanTermQueryDescriptor { get; set; }
		public ISpanFirstQuery SpanFirst { get; set; }
		public ISpanNearQuery SpanNear { get; set; }
		public ISpanOrQuery SpanOr { get; set; }
		public ISpanNotQuery SpanNot { get; set; }
	}

	public class SpanQuery<T> : ISpanQuery where T : class
	{
		ISpanTermQuery ISpanQuery.SpanTermQueryDescriptor { get; set; }

		ISpanFirstQuery ISpanQuery.SpanFirst { get; set; }

		ISpanNearQuery ISpanQuery.SpanNear { get; set; }

		ISpanOrQuery ISpanQuery.SpanOr { get; set; }

		ISpanNotQuery ISpanQuery.SpanNot { get; set; }

		bool IQuery.IsConditionless
		{
			get
			{
				var queries = new[]
				{
					((ISpanQuery)this).SpanTermQueryDescriptor as IQuery,
					((ISpanQuery)this).SpanFirst as IQuery,
					((ISpanQuery)this).SpanNear as IQuery,
					((ISpanQuery)this).SpanOr as IQuery,
					((ISpanQuery)this).SpanNot as IQuery
				};
				return queries.All(q => q == null || q.IsConditionless);
			}
		}

		public SpanQuery<T> SpanTerm(Expression<Func<T, object>> fieldDescriptor
			, string value	
			, double? Boost = null)
		{
			if (fieldDescriptor == null || value.IsNullOrEmpty())
				return this;

			var spanTerm = new SpanTermQueryDescriptor<T>();
			((ITermQuery)spanTerm).Field = fieldDescriptor;
			((ITermQuery)spanTerm).Value = value;
			((ITermQuery)spanTerm).Boost = Boost;
			return CreateQuery(spanTerm, (sq) => ((ISpanQuery)sq).SpanTermQueryDescriptor = spanTerm);
		}
		
		public SpanQuery<T> SpanTerm(string field, string value, double? Boost = null)
		{
			if (field.IsNullOrEmpty() || value.IsNullOrEmpty())
				return this;

			var spanTerm = new SpanTermQueryDescriptor<T>();
			((ITermQuery)spanTerm).Field = field;
			((ITermQuery)spanTerm).Value = value;
			((ITermQuery)spanTerm).Boost = Boost;
			return CreateQuery(spanTerm, (sq) => ((ISpanQuery)sq).SpanTermQueryDescriptor = spanTerm);

		}
		
		public SpanQuery<T> SpanFirst(Func<SpanFirstQueryDescriptor<T>, SpanFirstQueryDescriptor<T>> selector)
		{
			selector.ThrowIfNull("selector");
			var q = selector(new SpanFirstQueryDescriptor<T>());
			return CreateQuery(q, (sq) => ((ISpanQuery)sq).SpanFirst = q);
		}
		
		public SpanQuery<T> SpanNear(Func<SpanNearQueryDescriptor<T>, SpanNearQueryDescriptor<T>> selector)
		{
			selector.ThrowIfNull("selector");
			var q = selector(new SpanNearQueryDescriptor<T>());
			return CreateQuery(q, (sq) => ((ISpanQuery)sq).SpanNear = q);
		}
		public SpanQuery<T> SpanOr(Func<SpanOrQueryDescriptor<T>, SpanOrQueryDescriptor<T>> selector)
		{
			selector.ThrowIfNull("selector");
			var q = selector(new SpanOrQueryDescriptor<T>());
			return CreateQuery(q, (sq) => ((ISpanQuery)sq).SpanOr = q);
		}
		public SpanQuery<T> SpanNot(Func<SpanNotQuery<T>, SpanNotQuery<T>> selector)
		{
			selector.ThrowIfNull("selector");
			var q = selector(new SpanNotQuery<T>());
			return CreateQuery(q, (sq) => ((ISpanQuery)sq).SpanNot = q);
		}

		private SpanQuery<T> CreateQuery<K>(K query, Action<SpanQuery<T>> setProperty) where K : ISpanSubQuery
		{
			if (((IQuery)(query)).IsConditionless)
				return this;

			var newSpanQuery = new SpanQuery<T>();
			setProperty(newSpanQuery);
			return newSpanQuery;
		}
	}
}
