using System;
using CUE.NET;
using CUE.NET.Devices.Generic.Enums;

namespace Snakeboard
{
	internal class Program
	{
		private static void Main()
		{
			CueSDK.Initialize();

			if (CueSDK.IsSDKAvailable(CorsairDeviceType.Keyboard))
				new SnakeGame();
			else
				Console.WriteLine("Keyboard not detected!");

			Console.ReadKey(true);
		}
	}
}
