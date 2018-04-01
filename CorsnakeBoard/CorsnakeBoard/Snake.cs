using System;
using System.Collections.Generic;
using System.Linq;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace Snakeboard
{
	internal class Snake
	{
		public enum Directions
		{
			Up,
			Down,
			Left,
			Right,
			None
		}

		public int SnakeSize = 1;
		public List<CorsairLedId> SnakePositions;

		public static readonly CorsairColor SnakeColor = new CorsairColor(0, 255, 0);

		public Snake()
		{
			SnakePositions = new List<CorsairLedId>();
			SnakeSize = 1;
			SpawnSnake();
		}

		public void MoveSnake(Directions direction)
		{
			var head = SnakePositions.Last();
			CorsairLedId? tail = null;
			var key = SnakeGame.KeyboardLayout.GetNextKey(head, direction);

			if (key != null)
			{
				tail = SnakePositions.First();
				UpdateSnakePositions(tail.GetValueOrDefault(), key.GetValueOrDefault());
			}
			else
			{
				SnakeGame.EndGame();
			}

			var debug = false;
			if (debug) {
				Console.WriteLine("Moving Snake {");
				Console.WriteLine(" Direction: " + direction);
				Console.WriteLine(" Head: " + head);
				Console.WriteLine(" Tail: " + tail);
				Console.WriteLine(" NextKey: " + key);
				Console.WriteLine("}");
			}
		}

		private void UpdateSnakePositions(CorsairLedId previousKey, CorsairLedId newKey)
		{
			if (Food.FoodPositionKey == newKey)
			{
				GrowSnake();
			}
			else if (SnakePositions.Contains(newKey))
			{
				SnakeGame.EndGame();
			}
			else
			{
				SnakePositions.Remove(previousKey);
				SnakeGame.Keyboard[previousKey].Color = new CorsairColor(0, 0, 0);
			}
			UpdateSnakePositions(newKey);
		}

		private void UpdateSnakePositions(CorsairLedId newKey)
		{
			SnakePositions.Add(newKey);
			SnakeGame.Keyboard[newKey].Color = SnakeColor;
			SnakeGame.Keyboard.Update();

			if (Food.FoodPositionKey == null)
				Food.SpawnFood();
		}

		public void GrowSnake()
		{
			Food.DestroyFood();
			SnakeSize++;
		}

		public List<CorsairLedId> GetSnakePositions()
		{
			return SnakePositions;
		}

		private void SpawnSnake()
		{
			var randomKey = SnakeGame.KeyboardLayout.GetRandomKey();
			UpdateSnakePositions(randomKey);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Snake Spawned on " + randomKey);
			Console.ResetColor();
		}

	}
}