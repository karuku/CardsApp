using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Extensions
{
	public static class ObjectExtensions
	{
		public static bool IsNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}
		public static bool IsEmailAddress(this string value)
		{
			if( string.IsNullOrWhiteSpace(value)) return false;

			if (value.Length < 6) return false;
			if (!value.Contains("@") && !value.Contains(".")) return false;
			return true;
		}
		public static DateTime ToAppDateTime(this DateTime value)
		{
			return value.ToUniversalTime();
		}
		public static DateTime? ToDateTime(this string value)
		{
			string[] format = { "yyyyMMdd" };
			DateTime date;

			if (DateTime.TryParseExact(value,
									   format,
									   System.Globalization.CultureInfo.InvariantCulture,
									   System.Globalization.DateTimeStyles.None,
									   out date))
			{
				return date;
			}
			return null;
		}
		
		public static long ConvertToInt64(this object? value)
		{
			string val = value?.ToString();
			var isNum = long.TryParse(val, out var num);
			if (isNum) return num;
			return 0;
		}
		public static int ConvertToInt32(this object? value)
		{
			string val = value?.ToString();
			var isNum = int.TryParse(val, out var num);
			if (isNum) return num;
			return 0;
		}
		
	}
}
