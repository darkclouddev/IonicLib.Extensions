using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace IonicLib.Extensions
{
	public static class Extensions
	{
		public static T Random<T>(this IEnumerable<T> collection)
		{
			return collection.ToArray()[Helper.XorShift() % collection.Count()];
		}

		public static bool IsWeekEnd(this DateTime dt) =>
			dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday;

		public static TimeSpan GetElapsed(this Action action)
		{
			var sw = Stopwatch.StartNew();

			action();

			sw.Stop();

			return sw.Elapsed;
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> elements, Action<T> action)
		{
			foreach (var element in elements)
			{
				action(element);
			}

			return elements;
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
		{
			using (var provider = RandomNumberGenerator.Create())
			{
				var list = collection.ToList();
				var count = list.Count;

				while (count > 1)
				{
					var box = new byte[(count / Byte.MaxValue) + 1];
					int boxSum;

					do
					{
						provider.GetBytes(box);
						boxSum = box.Sum(x => x);
					}
					while (!(boxSum < count * ((Byte.MaxValue * box.Length) / count)));

					var pos = (boxSum % count);
					count--;

					var value = list[pos];
					list[pos] = list[count];
					list[count] = value;
				}

				return list;
			}
		}

		/// <param name="min">Inclusive minimum</param>
		/// <param name="max">Inclusive maximum</param>
		public static int LimitToRange(this int value, int min, int max)
		{
			if (value < min)
				return min;

			if (value > max)
				return max;

			return value;
		}

		public static string FormatTimeToString(this TimeSpan ts)
		{
			string result = "";

			if (ts.Days > 0)
				result += $"{ts.Days} д. ";

			if (ts.Hours > 0)
				result += $"{ts.Hours} ч. ";

			if (ts.Minutes > 0)
				result += $"{ts.Minutes} мин. ";

			if (ts.Seconds > 0)
				result += $"{ts.Seconds} сек.";

			return result;
		}

		public static string TrimTo(this string str, int maxLength = 50, bool showDots = true)
		{
			if (maxLength < 0)
				throw new ArgumentOutOfRangeException($"");

			if (maxLength == 0)
				return String.Empty;

			if (maxLength <= 3)
				return String.Concat(str.Select(x => '.'));

			if (str.Length < maxLength)
				return str;

			return String.Concat(str.Take(maxLength - 3)) + (showDots ? "..." : "");
		}

		public static DateTime ToUnixTimestamp(this double number) =>
			new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(number);

		public static uint AsUnixTimestamp(this DateTime dt) =>
			(uint)dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

		public static ulong ToUnixTimestamp(this DateTime dt)
		{
			return (uint)Math.Floor((dt - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
		}

		public static int KiB(this int value) => value * 1024;
		public static int KB(this int value) => value * 1000;

		public static int MiB(this int value) => value.KiB() * 1024;
		public static int MB(this int value) => value.KB() * 1000;

		public static int GiB(this int value) => value.MiB() * 1024;
		public static int GB(this int value) => value.MB() * 1000;

		public static ulong KiB(this ulong value) => value * 1024;
		public static ulong KB(this ulong value) => value * 1000;

		public static ulong MiB(this ulong value) => value.KiB() * 1024;
		public static ulong MB(this ulong value) => value.KB() * 1000;

		public static ulong GiB(this ulong value) => value.MiB() * 1024;
		public static ulong GB(this ulong value) => value.MB() * 1000;
	}
}