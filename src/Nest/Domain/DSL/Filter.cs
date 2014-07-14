﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nest.Resolvers.Converters;
using System.Linq.Expressions;

namespace Nest
{

	public static class Filter<T> where T : class
	{
		public static FilterContainer And(params Func<FilterDescriptor<T>, FilterContainer>[] filters)
		{
			return new FilterDescriptor<T>().And(filters);
		}
		public static FilterContainer Bool(Action<BoolFilterDescriptor<T>> booleanFilter)
		{
			return new FilterDescriptor<T>().Bool(booleanFilter);
		}
		public static FilterContainer Exists(Expression<Func<T, object>> fieldDescriptor)
		{
			return new FilterDescriptor<T>().Exists(fieldDescriptor);
		}
		public static FilterContainer Exists(string field)
		{
			return new FilterDescriptor<T>().Exists(field);
		}
		public static FilterContainer Conditionless(Action<ConditionlessFilterDescriptor<T>> conditionlessFilter)
		{
			return new FilterDescriptor<T>().Conditionless(conditionlessFilter);
		}
		public static FilterContainer GeoBoundingBox(Expression<Func<T, object>> fieldDescriptor, double topLeftX, double topLeftY, double bottomRightX, double bottomRightY, GeoExecution? Type = null)
		{
			return new FilterDescriptor<T>().GeoBoundingBox(fieldDescriptor, topLeftX, topLeftY, bottomRightX, bottomRightY, Type);
		}
		public static FilterContainer GeoBoundingBox(Expression<Func<T, object>> fieldDescriptor, string geoHashTopLeft, string geoHashBottomRight, GeoExecution? Type = null)
		{
			return new FilterDescriptor<T>().GeoBoundingBox(fieldDescriptor, geoHashTopLeft, geoHashBottomRight, Type);
		}
		public static FilterContainer GeoBoundingBox(string fieldName, double topLeftX, double topLeftY, double bottomRightX, double bottomRightY, GeoExecution? Type = null)
		{
			return new FilterDescriptor<T>().GeoBoundingBox(fieldName, topLeftX, topLeftY, bottomRightX, bottomRightY, Type);
		}
		public static FilterContainer GeoBoundingBox(string fieldName, string geoHashTopLeft, string geoHashBottomRight, GeoExecution? Type = null)
		{
			return new FilterDescriptor<T>().GeoBoundingBox(fieldName, geoHashTopLeft, geoHashBottomRight, Type);
		}
		public static FilterContainer GeoDistance(Expression<Func<T, object>> fieldDescriptor, Action<GeoDistanceFilterDescriptor> filterDescriptor)
		{
			return new FilterDescriptor<T>().GeoDistance(fieldDescriptor, filterDescriptor);
		}
		public static FilterContainer GeoDistance(string field, Action<GeoDistanceFilterDescriptor> filterDescriptor)
		{
			return new FilterDescriptor<T>().GeoDistance(field, filterDescriptor);
		}
		public static FilterContainer GeoDistanceRange(Expression<Func<T, object>> fieldDescriptor, Action<GeoDistanceRangeFilterDescriptor> filterDescriptor)
		{
			return new FilterDescriptor<T>().GeoDistanceRange(fieldDescriptor, filterDescriptor);
		}
		public static FilterContainer GeoDistanceRange(string field, Action<GeoDistanceRangeFilterDescriptor> filterDescriptor)
		{
			return new FilterDescriptor<T>().GeoDistanceRange(field, filterDescriptor);
		}
		public static FilterContainer GeoPolygon(Expression<Func<T, object>> fieldDescriptor, params string[] points)
		{
			return new FilterDescriptor<T>().GeoPolygon(fieldDescriptor, points);
		}
		public static FilterContainer GeoPolygon(Expression<Func<T, object>> fieldDescriptor, IEnumerable<Tuple<double, double>> points)
		{
			return new FilterDescriptor<T>().GeoPolygon(fieldDescriptor, points);
		}
		public static FilterContainer GeoPolygon(string field, IEnumerable<Tuple<double, double>> points)
		{
			return new FilterDescriptor<T>().GeoPolygon(field, points);
		}
		public static FilterContainer GeoPolygon(string fieldName, params string[] points)
		{
			return new FilterDescriptor<T>().GeoPolygon(fieldName, points);
		}
		public static FilterContainer HasChild<K>(Action<HasChildFilterDescriptor<K>> querySelector) where K : class
		{
			return new FilterDescriptor<T>().HasChild<K>(querySelector);
		}
		public static FilterContainer Ids(IEnumerable<string> types, IEnumerable<string> values)
		{
			return new FilterDescriptor<T>().Ids(types, values);
		}
		public static FilterContainer Ids(IEnumerable<string> values)
		{
			return new FilterDescriptor<T>().Ids(values);
		}
		public static FilterContainer Ids(string type, IEnumerable<string> values)
		{
			return new FilterDescriptor<T>().Ids(type, values);
		}
		public static FilterContainer Limit(int limit)
		{
			return new FilterDescriptor<T>().Limit(limit);
		}
		public static FilterContainer MatchAll()
		{
			return new FilterDescriptor<T>().MatchAll();
		}
		public static FilterContainer Missing(Expression<Func<T, object>> fieldDescriptor)
		{
			return new FilterDescriptor<T>().Missing(fieldDescriptor);
		}
		public static FilterContainer Missing(string field)
		{
			return new FilterDescriptor<T>().Missing(field);
		}
		public static FilterContainer Nested(Action<NestedFilterDescriptor<T>> selector)
		{
			return new FilterDescriptor<T>().Nested(selector);
		}
		public static FilterContainer Not(Func<FilterDescriptor<T>, FilterContainer> selector)
		{
			return new FilterDescriptor<T>().Not(selector);
		}
		public static FilterContainer Or(params Func<FilterDescriptor<T>, FilterContainer>[] filters)
		{
			return new FilterDescriptor<T>().Or(filters);
		}
		public static FilterContainer Prefix(Expression<Func<T, object>> fieldDescriptor, string prefix)
		{
			return new FilterDescriptor<T>().Prefix(fieldDescriptor, prefix);
		}
		public static FilterContainer Prefix(string field, string prefix)
		{
			return new FilterDescriptor<T>().Prefix(field, prefix);
		}
		public static FilterContainer Query(Func<QueryDescriptor<T>, QueryContainer> querySelector)
		{
			return new FilterDescriptor<T>().Query(querySelector);
		}
		public static FilterContainer Range(Action<RangeFilterDescriptor<T>> rangeSelector)
		{
			return new FilterDescriptor<T>().Range(rangeSelector);
		}
		public static FilterContainer Script(Action<ScriptFilterDescriptor> scriptSelector)
		{
			return new FilterDescriptor<T>().Script(scriptSelector);
		}
		public static FilterContainer Term<K>(Expression<Func<T, K>> fieldDescriptor, K term)
		{
			return new FilterDescriptor<T>().Term(fieldDescriptor, term);
		}
		public static FilterContainer Term<K>(string field, K term)
		{
			return new FilterDescriptor<T>().Term(field, term);
		}
		public static FilterContainer Terms(Expression<Func<T, object>> fieldDescriptor, IEnumerable<string> terms, TermsExecution? Execution = null)
		{
			return new FilterDescriptor<T>().Terms(fieldDescriptor, terms, Execution);
		}
		public static FilterContainer Terms(string field, IEnumerable<string> terms, TermsExecution? Execution = null)
		{
			return new FilterDescriptor<T>().Terms(field, terms, Execution);
		}
		public static FilterContainer Type(string type)
		{
			return new FilterDescriptor<T>().Type(type);
		}
		public static FilterContainer Regexp(Action<RegexpFilterDescriptor<T>> regexpSelector)
		{
			return new FilterDescriptor<T>().Regexp(regexpSelector);
		}
		public static FilterDescriptor<T>  Strict(bool strict = true)
		{	
			var f = new FilterDescriptor<T>();
			((IFilterContainer)f).IsStrict = true;
			return f;
		}
		public static FilterDescriptor<T>  Verbatim(bool verbatim = true)
		{
			var f = new FilterDescriptor<T>();
			((IFilterContainer)f).IsVerbatim = true;
			return f;
		}
		
	}

}
