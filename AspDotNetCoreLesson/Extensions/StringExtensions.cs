using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Extensions
{
	public static class StringExtensions
	{
		public static string ToCamel(this string param) =>
			$"{char.ToLowerInvariant(param[0])}{param.Substring(1)}";
	}
}