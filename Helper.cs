using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IonicLib.Extensions
{
    public static class Helper
    {
		static readonly Random random = new Random();

		const string symbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static string GenerateConfirmationCode(int length)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < length; i++)
			{
				sb.Append(symbols.Random());
			}

			return sb.ToString();
		}

		public static bool GetChance(uint chance) => GetChance((ulong)chance);
		public static bool GetChance(ulong chance) => (ulong)random.Next(1, 101) <= chance;

		/// <summary>
		/// See Random.Next(int,int)
		/// </summary>
		/// <param name="min">Minimum value, inclusive</param>
		/// <param name="max">Maximum value, exclusive</param>
		/// <returns>A 32-bit unsigned integer greater than or equal to minValue and less than maxValue;
		/// that is, the range of return values includes minValue but not maxValue.
		/// If minValue equals maxValue, minValue is returned.
		/// </returns>
		public static uint NextUInt(int min, int max) => (uint)random.Next(min, max);

		public static bool GetPreciseChance(float chance) => random.Next(1, 10001) / 100f <= chance;

		static uint x = 548787455, y = 842502087, z = 3579807591, w = 273326509;

		public static uint XorShift()
		{
			uint t = x ^ (x << 11);
			x = y;
			y = z;
			z = w;

			return w = w ^ (w >> 19) ^ t ^ (t >> 8);
		}

		public static ulong CurrentUnixTimestamp => (ulong)Math.Floor((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);

		public static DateTime FromTimestamp(ulong unixTimestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimestamp);
		}

		const int ProcessorCountRefreshIntervalMs = 30000;

		static volatile int _processorCount;
		static volatile int _lastProcessorCountRefreshTicks;

		internal static int ProcessorCount
		{
			get
			{
				int now = Environment.TickCount;

				if (_processorCount == 0 || now - _lastProcessorCountRefreshTicks >= ProcessorCountRefreshIntervalMs)
				{
					_processorCount = Environment.ProcessorCount;
					_lastProcessorCountRefreshTicks = now;
				}

				return _processorCount;
			}
		}

		public async static Task<string> PostAsync(string address, Dictionary<string, string> parameters)
		{
			using (var client = new HttpClient())
			{
				var @params = new FormUrlEncodedContent(parameters);

				var response = await client.PostAsync(address, @params);

				return await response.Content.ReadAsStringAsync();
			}
		}
	}
}