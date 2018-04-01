using System;
using System.Threading;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;

namespace Snakeboard
{
	internal class SnakeGame
	{
		public static CorsairKeyboard Keyboard;
		public static KeyboardLayout KeyboardLayout;
		public static Snake Snake;
		public static Random Rand = new Random();

		private readonly CorsairColor _movementKeyColor = new CorsairColor(0, 215, 132);

		private static Thread _movementThread;
		private static Thread _tickerThread;

		protected int MovementTimer = 250;
		private Snake.Directions _currentMovementDirection = Snake.Directions.None;

		public SnakeGame()
		{
			Keyboard = CueSDK.KeyboardSDK;

			KeyboardLayout = new KeyboardLayout();

			// Allow manual color changes
			Keyboard.Brush = (SolidColorBrush) CorsairColor.Transparent;

			ShowStartScreen();

			SetupThreads();
		}

		protected void SetupThreads()
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;

			Thread.CurrentThread.Name = "MainThread";
			Console.WriteLine("Game is running on {0}", Thread.CurrentThread.Name);

			_movementThread = new Thread(MovementThread)
			{
				Name = "MovementThread"
			};
			Console.WriteLine("Movement Listener Thread is running");
			_movementThread.Start();

			_tickerThread = new Thread(TickerThread)
			{
				Name = "TickerThread"
			};
			Console.WriteLine("Ticker Thread is running");
			_tickerThread.Start();

			Console.ResetColor();
		}

		protected void MovementThread()
		{

			while (true)
			{
				var key = Console.ReadKey(true).Key;
				if (
					key == ConsoleKey.LeftArrow
					|| key == ConsoleKey.RightArrow
					|| key == ConsoleKey.UpArrow
					|| key == ConsoleKey.DownArrow
				)
				{
					//_snake.MoveSnake(GetDirection(key).GetValueOrDefault());
					_currentMovementDirection = GetDirection(key).GetValueOrDefault();
				}
			}

			Snake.Directions? GetDirection(ConsoleKey pressedKey)
			{
				switch (pressedKey)
				{
					case ConsoleKey.LeftArrow:
						return Snake.Directions.Left;
					case ConsoleKey.RightArrow:
						return Snake.Directions.Right;
					case ConsoleKey.DownArrow:
						return Snake.Directions.Down;
					case ConsoleKey.UpArrow:
						return Snake.Directions.Up;
					default:
						return null;
				}
			}
		}

		protected void TickerThread()
		{
			while (true)
			{
				if (_currentMovementDirection != Snake.Directions.None)
					Snake.MoveSnake(_currentMovementDirection);
				Food.UpdateColor();
				Thread.Sleep(MovementTimer);
			}
		}

		protected void ShowStartScreen()
		{
			var backgroundColor = new CorsairColor(0, 0, 0);
			var textColor1 = new CorsairColor(255, 255, 0);
			var startKeyColor = new CorsairColor(0, 255, 0);

			var startKey = CorsairLedId.Space;

			SetColorToAllKeys(backgroundColor);
			
			// Color the start key to its color
			Keyboard[startKey].Color = startKeyColor;
			Keyboard[CorsairLedId.T].Color = startKeyColor;
			
			// Write out ssss...
			// -= S =-
			Keyboard[CorsairLedId.F2].Color = textColor1;
			Keyboard[CorsairLedId.F1].Color = textColor1;
			Keyboard[CorsairLedId.D1].Color = textColor1;
			Keyboard[CorsairLedId.Q].Color = textColor1;
			Keyboard[CorsairLedId.W].Color = textColor1;
			Keyboard[CorsairLedId.S].Color = textColor1;
			Keyboard[CorsairLedId.Z].Color = textColor1;
			Keyboard[CorsairLedId.NonUsBackslash].Color = textColor1;

			// TODO Complete writing out S's

			Keyboard.Update();

			Console.WriteLine("Welcome to snake!");
			Console.WriteLine("[T]     - Test Keys");
			Console.WriteLine("[Space] - Start Game");

			var keyInfo = Console.ReadKey(true);
			Console.Clear();
			if (keyInfo.Key == ConsoleKey.T)
			{
				KeyboardLayout.TestGameBoard();
			}
			else if (keyInfo.Key == ConsoleKey.Spacebar)
			{
				StartGame();
			}
		}

		public static void SetColorToAllKeys(CorsairColor color)
		{
			var leds = Keyboard.GetLeds();
			foreach (var corsairLed in leds)
			{
				corsairLed.Color = color;
			}
			Keyboard.Update();
		}

		public void StartGame()
		{
			SetColorToAllKeys(new CorsairColor(0, 0, 0));
			ColorMovementKeys();

			SpawnSnake();
			SpawnFood();
		}

		protected void ColorMovementKeys()
		{
			Keyboard[CorsairLedId.UpArrow].Color = _movementKeyColor;
			Keyboard[CorsairLedId.LeftArrow].Color = _movementKeyColor;
			Keyboard[CorsairLedId.RightArrow].Color = _movementKeyColor;
			Keyboard[CorsairLedId.DownArrow].Color = _movementKeyColor;

			Keyboard.Update();
		}

		protected void SpawnFood()
		{
			Food.SpawnFood();
		}

		protected void SpawnSnake()
		{
			Snake = new Snake();
		}

		public static void EndGame()
		{
			Console.WriteLine("You Died!");
			_movementThread.Abort();
			_tickerThread.Abort();
			CueSDK.Reinitialize();
		}

	}
}
