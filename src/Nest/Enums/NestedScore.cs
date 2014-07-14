﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Nest
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum NestedScore
	{
		[EnumMember(Value = "avg")]
		Average,
		[EnumMember(Value = "total")]
		Total,
		[EnumMember(Value = "max")]
		Max,
		[EnumMember(Value = "none")]
		None
	}
}
