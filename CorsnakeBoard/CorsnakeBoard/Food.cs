using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace Snakeboard
{
	class Food
	{
		public static CorsairLedId? FoodPositionKey = null;
		public static readonly CorsairColor FoodColor = new CorsairColor(255, 0, 0);

		public static void SpawnFood()
		{
			if (FoodPositionKey != null)
				SnakeGame.Keyboard[FoodPositionKey.GetValueOrDefault()].Color = new CorsairColor(0, 0, 0);

			var randomKey = SnakeGame.KeyboardLayout.GetRandomUnusedKey();
			SnakeGame.Keyboard[randomKey].Color = FoodColor;
			FoodPositionKey = randomKey;

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Food Spawned on {0}", randomKey);
			Console.ResetColor();

			SnakeGame.Keyboard.Update();
		}

		public static void DestroyFood()
		{
			SnakeGame.Keyboard[FoodPositionKey.GetValueOrDefault()].Color = new CorsairColor(0, 0, 0);
			FoodPositionKey = null;
			SnakeGame.Keyboard.Update();
		}

		public static void UpdateColor()
		{
			if (FoodPositionKey != null)
			{
				if (SnakeGame.Keyboard[FoodPositionKey.GetValueOrDefault()].Color != FoodColor)
					SnakeGame.Keyboard[FoodPositionKey.GetValueOrDefault()].Color = FoodColor;
			}
		}

	}
}
