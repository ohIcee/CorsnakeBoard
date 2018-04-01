using System;
using System.Collections.Generic;
using CUE.NET;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace Snakeboard
{
	internal class KeyboardLayout
	{

		private readonly Dictionary<int, List<CorsairLedId>> _rowKeys = new Dictionary<int, List<CorsairLedId>>();

		public KeyboardLayout()
		{
			FillKeysDict();
		}

		public Dictionary<int, List<CorsairLedId>> GetRowKeys()
		{
			return _rowKeys;
		}

		protected void FillKeysDict()
		{

			_rowKeys[0] = new List<CorsairLedId>
			{
				// Row 1
				CorsairLedId.D1,
				CorsairLedId.D2,
				CorsairLedId.D3,
				CorsairLedId.D4,
				CorsairLedId.D5,
				CorsairLedId.D6,
				CorsairLedId.D7,
				CorsairLedId.D8,
				CorsairLedId.D9,
				CorsairLedId.D0,
				CorsairLedId.MinusAndUnderscore,
				CorsairLedId.EqualsAndPlus
			};

			_rowKeys[1] = new List<CorsairLedId>
			{
				// Row 2
				CorsairLedId.Q,
				CorsairLedId.W,
				CorsairLedId.E,
				CorsairLedId.R,
				CorsairLedId.T,
				CorsairLedId.Y,
				CorsairLedId.U,
				CorsairLedId.I,
				CorsairLedId.O,
				CorsairLedId.P,
				CorsairLedId.BracketLeft
			};

			_rowKeys[2] = new List<CorsairLedId>
			{
				// Row 3
				CorsairLedId.A,
				CorsairLedId.S,
				CorsairLedId.D,
				CorsairLedId.F,
				CorsairLedId.G,
				CorsairLedId.H,
				CorsairLedId.J,
				CorsairLedId.K,
				CorsairLedId.L,
				CorsairLedId.SemicolonAndColon,
				CorsairLedId.ApostropheAndDoubleQuote,
				CorsairLedId.NonUsBackslash
			};

			_rowKeys[3] = new List<CorsairLedId>
			{
				// Row 4
				CorsairLedId.Z,
				CorsairLedId.X,
				CorsairLedId.C,
				CorsairLedId.V,
				CorsairLedId.B,
				CorsairLedId.N,
				CorsairLedId.M,
				CorsairLedId.CommaAndLessThan,
				CorsairLedId.PeriodAndBiggerThan,
				CorsairLedId.SlashAndQuestionMark
			};

		}

		public void TestGameBoard()
		{
			var rnd = new Random();

			SnakeGame.SetColorToAllKeys(new CorsairColor(0, 0, 0));

			Console.WriteLine("-=== TESTING KEYBOARD ===-");

			for (var i = 0; i < _rowKeys.Count; i++)
			{
				var row = _rowKeys[i];
				Console.WriteLine("--- Row {0} ---", i);
				foreach (var key in row)
				{
					var randomRed = (byte) rnd.Next(0, 255);
					var randomGreen = (byte) rnd.Next(0, 255);
					var randomBlue = (byte) rnd.Next(0, 255);

					SnakeGame.Keyboard[key].Color = new CorsairColor(randomRed, randomGreen, randomBlue);
					SnakeGame.Keyboard.Update();
					Console.WriteLine("Testing Key {0}", key);
					System.Threading.Thread.Sleep(100);
				}
			}

			Console.WriteLine("-=== KEYBOARD TEST COMPLETED ===-");

		}

		public List<CorsairLedId> GetLeds(int row)
		{
			return _rowKeys[row];
		}

		public CorsairLedId GetRandomKey()
		{
			var randomRow = SnakeGame.Rand.Next(0, _rowKeys.Count);
			var randomKey = _rowKeys[randomRow][SnakeGame.Rand.Next(0, _rowKeys[randomRow].Count)];
			return randomKey;
		}

		public CorsairLedId GetRandomUnusedKey()
		{
			do
			{
				var randomRow = SnakeGame.Rand.Next(0, _rowKeys.Count);
				var randomKey = _rowKeys[randomRow][SnakeGame.Rand.Next(0, _rowKeys[randomRow].Count)];
				return randomKey;		
			} while (SnakeGame.Snake.SnakePositions.Exists(x => x == GetRandomUnusedKey()));
		}

		public int[] GetRowAndKeyIndex(CorsairLedId key)
		{
			var rowAndKeyIndex = new int[2];

			for (var rowIndex = 0; rowIndex < _rowKeys.Count; rowIndex++)
			{
				for (var keyIndex = 0; keyIndex < _rowKeys[rowIndex].Count; keyIndex++)
				{
					if (_rowKeys[rowIndex][keyIndex] == key)
					{
						rowAndKeyIndex[0] = rowIndex;
						rowAndKeyIndex[1] = keyIndex;
						return rowAndKeyIndex;
					}
				}
			}

			return null;
		}

		protected CorsairLedId? GetKeyAtIndex(int[] keyAndRowIndex)
		{
			
			for (var rowIndex = 0; rowIndex < _rowKeys.Count; rowIndex++)
			{
				for (var keyIndex = 0; keyIndex < _rowKeys[rowIndex].Count; keyIndex++)
				{
					if (
						keyAndRowIndex[0] == rowIndex
						&& keyAndRowIndex[1] == keyIndex
					) return _rowKeys[rowIndex][keyIndex];
				}
			}

			return null;
		}

		public CorsairLedId? GetNextKey(CorsairLedId originKey, Snake.Directions direction)
		{
			var rowAndKeyIndex = GetRowAndKeyIndex(originKey);
			var originRowIndex = rowAndKeyIndex[0];
			var originKeyIndex = rowAndKeyIndex[1];

			switch (direction)
			{
				case Snake.Directions.Up:
					return GetKeyAtIndex(
							new int[]
							{
								originRowIndex - 1, 
								originKeyIndex
							}
						);
				case Snake.Directions.Left:
					return GetKeyAtIndex(
						new int[]
						{
							originRowIndex, 
							originKeyIndex - 1
						}
					);
				case Snake.Directions.Right:
					return GetKeyAtIndex(
						new int[]
						{
							originRowIndex, 
							originKeyIndex + 1
						}
					);
				case Snake.Directions.Down:
					return GetKeyAtIndex(
						new int[]
						{
							originRowIndex + 1, 
							originKeyIndex
						}
					);
				default:
					return null;
			}
		}

	}
}