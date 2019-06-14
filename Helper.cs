using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IonicLib.Extensions
{
    public static class Helper
    {
		static readonly Random random = new Random();

		static readonly char[] symbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

		public static string GenerateConfirmationCode(int length)
		{
			var sb = new StringBuilder();

			for (var i = 0; i < length; i++)
			{
				sb.Append(symbols.Random());
			}

			return sb.ToString();
		}

		public static bool GetChance(uint chance) => GetChance((ulong)chance);
		public static bool GetChance(ulong chance) => (ulong)random.Next(1, 101) <= chance;

        public static int NextInt(int min, int max) => random.Next(min, max);

        /// <summary>
        /// @see Random.Next(int,int)
        /// </summary>
        /// <param name="min">Minimum value, inclusive</param>
        /// <param name="max">Maximum value, exclusive</param>
        /// <returns>A 32-bit unsigned integer greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue.
        /// If minValue equals maxValue, minValue is returned.
        /// </returns>
        public static uint NextUInt(int min, int max) => (uint)random.Next(min, max);

		public static bool GetPreciseChance(float chance) => random.Next(1, 10001) / 100f <= chance;

		public static ulong CurrentUnixTimestamp => (ulong)Math.Floor((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);

		public static DateTime FromTimestamp(ulong unixTimestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimestamp);
		}

		public static async Task<string> PostAsync(string address, Dictionary<string, string> parameters)
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