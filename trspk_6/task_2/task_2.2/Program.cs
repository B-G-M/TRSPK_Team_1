﻿using System;
using System.Text;

namespace task2_2
{
	class LongNumber
	{
		//public LongNumber()
		//{
		//	number = "";
		//}
		public LongNumber(string num)
		{
			number = num;
		}
		public LongNumber(int num)
		{
			number = Convert.ToString(num);
		}
		public LongNumber(LongNumber longNumber)
		{
			number = longNumber.number;
		}

		private string number;
		public string Number
		{
			get { return number; }
			set { number = value; }
		}

		private static bool cycle = false;

		public static string operator +(LongNumber number1, LongNumber number2)
		{
			LongNumber sum = new();

			if (!cycle)
			{
				cycle = true;
				if (number1.number[0] == '-' && number2.number[0] != '-')
				{
					sum.number = number1 - number2;
					sum.number = sum.number.Insert(0, Convert.ToString('-'));

					if (sum.number[0] == '-' && sum.number[1] == '-')
					{
						Two_minus_to_plus(sum);
					}
					return sum.number;
				}
				if (number1.number[0] != '-' && number2.number[0] == '-')
				{
					return number1 - number2;
				}
			}
			if (number1.number.Length > number2.number.Length)
			{
				Swap(number1,number2);
			}

			int temp1 = 0,
				temp2 = 0,
				num1_lenght = number1.number.Length,
				num2_lenght = number2.number.Length;

			for (; num1_lenght > 0;)
			{
				num1_lenght--;
				num2_lenght--;
				if (number1.number[num1_lenght] == '-' || number2.number[num2_lenght] == '-') { break; }

				temp1 += (number1.number[num1_lenght] - '0');
				temp2 += (number2.number[num2_lenght] - '0');

				if (temp1 + temp2 <= 9)
				{
					sum.number = sum.number.Insert(0, Convert.ToString(temp1 + temp2));
					temp1 = 0;
					temp2 = 0;
					continue;
				}
				temp1 += temp2;
				temp1 %= 10;
				temp2 = 1;
				sum.number = sum.number.Insert(0, Convert.ToString(temp1));
				temp1 = 0;

				if (num2_lenght == 0 && temp2 != 0)
				{
					sum.number = sum.number.Insert(0, Convert.ToString(temp2));
				}
			}

			for (; 0 < num2_lenght;)
			{
				num2_lenght--;
				if (number2.number[num2_lenght] == '-') { break; }

				temp2 += (number2.number[num2_lenght] - '0');

				if (temp2 <= 9 || num2_lenght == 0)
				{
					sum.number = sum.number.Insert(0, Convert.ToString(temp2));
					temp2 = 0;
					continue;
				}
				temp2 %= 10;
				sum.number = sum.number.Insert(0, Convert.ToString(temp2));
				temp2 = 1;
			}

			if (number1.number[0] == '-' && number2.number[0] == '-')
			{
				sum.number = sum.number.Insert(0, Convert.ToString('-'));
			}
			cycle = false;
			return sum.number;
		}
		public static string operator -(LongNumber number1, LongNumber number2)
		{
			LongNumber sub = new();
			bool less = false;

			if (!cycle)
			{
				cycle = true;
				if (number1.number[0] == '-' && number2.number[0] != '-')
				{
					sub.number = number1 + number2;
					sub.number = sub.number.Insert(0, Convert.ToString('-'));
					return sub.number;
				}
				if (number1.number[0] != '-' && number2.number[0] == '-')
				{
					return number1 + number2;
				}
			}

			if (Mod(number1) < Mod(number2))
			{
				less = true;
				Swap(number1,number2);
			}

			if (number1 == number2)
			{
				sub = "0";
				return sub.number;
			}

			int temp1,
				temp2,
				num1_lenght = number1.number.Length,
				num2_lenght = number2.number.Length,
				i = num1_lenght < num2_lenght ? num1_lenght : num2_lenght;

			for (; i > 0; i--)
			{
				if (number1.number[num1_lenght - 1] == '-' || number2.number[num2_lenght - 1] == '-') { break; }
				num1_lenght--;
				num2_lenght--;

				temp1 = (number1.number[num1_lenght] - '0');
				temp2 = (number2.number[num2_lenght] - '0');

				if (temp1 - temp2 >= 0)
				{
					if (temp1 - temp2 == 0 && i == 1)
					{
						continue;
					}
					sub.number = sub.number.Insert(0, Convert.ToString(temp1 - temp2));
					continue;
				}
				Give_ten(number1, num1_lenght);
				temp1 += 10;
				if (temp1 - temp2 <= 9 && i == 1)
				{
					num1_lenght--;
				}
				temp1 -= temp2;
				sub.number = sub.number.Insert(0, Convert.ToString(temp1));
			}
			for (; 0 < num1_lenght;)
			{
				num1_lenght--;
				if (number1.number[num1_lenght] == '-') { break; };
				temp1 = (number1.number[num1_lenght] - '0');
				sub.number = sub.number.Insert(0, Convert.ToString(temp1));
			}
			while (sub.number[0] == '0')
			{
				sub.number = sub.number.Remove(0, 1);
			}
			if (less)
			{
				sub.number = sub.number.Insert(0, Convert.ToString('-'));
			}

			if (number1.number[0] == '-' && number2.number[0] == '-')
			{
				sub.number = sub.number.Insert(0, Convert.ToString('-'));
			}

			if (sub.number[0] == '-' && sub.number[1] == '-')
			{
				Two_minus_to_plus(sub);
			}
			cycle = false;
			return sub.number;
		}
		public static string operator *(LongNumber number1, LongNumber number2)
		{
			LongNumber mult = new(0);
			LongNumber mult_rez = new();

			int temp1,
				temp2 = 0;

			if (number1.number[0] == '0' || number2.number[0] == '0')
			{
				mult.number = "0";
				return mult.number;
			}

			if (number1.number.Length < number2.number.Length)
			{
				Swap(number1,number2);
			}

			for (int i = number2.number.Length - 1; i >= 0; i--)
			{
				if (number2.number[i] == '-') { break; };

				for (int j = number1.number.Length - 1; j >= 0; j--)
				{
					if (number1.number[j] == '-') { break; };
					temp1 = (number1.number[j] - '0') * (number2.number[i] - '0');
					temp1 += temp2;
					temp2 = temp1 / 10;
					temp1 %= 10;
					mult_rez.number = mult_rez.number.Insert(0, Convert.ToString(temp1));
					if (j - 1 < 0 && temp2 != 0)
					{
						mult_rez.number = mult_rez.number.Insert(0, Convert.ToString(temp2));
					}
				}
				temp2 = 0;
				int k = number2.number.Length - i;
				while (k > 1)
				{
					mult_rez.number += "0";
					k--;
				}

				mult = new(mult + mult_rez);
				mult_rez.number = "";
			}

			if (number1.number[0] == '-' && number2.number[0] != '-' || number1.number[0] != '-' && number2.number[0] == '-')
			{
				mult.number = mult.number.Insert(0, Convert.ToString('-'));
			}
			return mult.number;

		}
		public static string operator /(LongNumber number1, LongNumber number2)
		{
			LongNumber div = new();
			LongNumber temp1 = new();
			LongNumber temp2 = new();

			char[] arr1 = number1.number.ToCharArray();

			bool positive = true,
				changed_sign = false;

			if (Mod(number1) < Mod(number2))
			{
				return div.number = "0";
			}
			if (number1.number[0] == '-' && number2.number[0] != '-' || number1.number[0] != '-' && number2.number[0] == '-')
			{
				positive = false;
				if (number2.number[0] == '-')
				{
					changed_sign = true;
					number2.number = number2.number.Insert(0, Convert.ToString('-'));
					Two_minus_to_plus(number2);
				};
			}

			for (int i = 0; i < number1.number.Length; i++)
			{
				temp1.number += arr1[i];

				if (Mod(temp1) > Mod(number2))
				{
					for (int j = 1; ; j++)
					{
						temp2 = new(number2);
						temp2 *= j;
						if (temp1 < temp2)
						{
							j--;
							temp2 = new(number2);
							temp2 *= j;
							if (temp1 == temp2)
							{
								temp1.number = "";
								div.number += j;
								break;
							}
							temp1 -= temp2;
							div.number += j;
							break;
						}
					}
				}
			}
			temp1 = new(div);
			temp2 = new(number2);
			temp2 *= temp1;
			if (temp2 < number1)
			{
				while (temp2 < number1)
				{
					div.number += "0";
					temp1 = new(div);
					temp2 = new(number2);
					temp2 *= temp1;
				}
				div.number = div.number.Remove(div.number.Length - 1);
			}
			
			if (!positive)
			{
				if (changed_sign)
				{
					number2.number = number2.number.Insert(0, Convert.ToString('-'));
				}
				div.number = div.number.Insert(0, Convert.ToString('-'));
			}

			return div.number;
		}
		public static bool operator <(LongNumber number1, LongNumber number2)
		{
			bool da = true,
				positive = true;
			int i = 0,
				temp1,
				temp2;

			if (number1.number[0] == '-' && number2.number[0] != '-')
			{
				return da;
			}

			if (number2.number[0] == '-' && number1.number[0] != '-')
			{
				return !da;
			}

			if (number2.number[0] == '-' && number1.number[0] == '-')
			{
				positive = !positive;
				i++;
			}

			if (number1.number.Length < number2.number.Length)
			{
				if (positive)
				{
					return da;
				}
				return !da;
			}
			else if (number1.number.Length > number2.number.Length)
			{
				if (positive)
				{
					return !da;
				}
				return da;
			}

			for (; i < number1.number.Length; i++)
			{
				temp1 = (number1.number[i] - '0');
				temp2 = (number2.number[i] - '0');
				if (temp1 == temp2){continue;}

				if (temp1 < temp2)
				{
					if (positive)
					{
						return da;
					}
					return !da;
				}
				if (temp1 > temp2)
				{
					if (positive)
					{
						return !da;
					}
					return da;
				}
			}
			return !da;
		}
		public static bool operator >(LongNumber number1, LongNumber number2)
		{
			return !(number1 < number2);
		}
		public static bool operator ==(LongNumber number1, LongNumber number2)
		{
			if (number1.number ==  number2.number)
			{
				return true;
			}
			return false;
		}
		public static bool operator !=(LongNumber number1, LongNumber number2)
		{
			return !(number1 == number2);
		}

		public static implicit operator LongNumber(int number)
		{
			return new LongNumber (number);
		}
		public static implicit operator LongNumber(string number)
		{
			return new LongNumber(number);
		}
		public static implicit operator LongNumber(long number)
		{
			return new LongNumber(Convert.ToString(number));
		}
		public static implicit operator LongNumber(short number)
		{
			return new LongNumber(Convert.ToString(number));
		}
		public static implicit operator LongNumber(bool number)
		{
			if (number)
			{
				return new LongNumber("1");
			}
			return new LongNumber("0");
		}

		public static explicit operator int(LongNumber number)
		{
			return Convert.ToInt32(number.number);
		}
		public static explicit operator string(LongNumber number)
		{
			return number.number;
		}
		public static explicit operator long(LongNumber number)
		{
			return Convert.ToInt64(number.number);
		}
		public static explicit operator short(LongNumber number)
		{
			return Convert.ToInt16(number.number);
		}
		public static explicit operator bool(LongNumber number)
		{
			if (number != "0")
			{
				return true;
			}
			return false;
		}

		public static LongNumber ToLongNumber(string number)
		{
			return new LongNumber(number);
		}
		public static LongNumber ToLongNumber(StringBuilder number)
		{
			return new LongNumber(Convert.ToString(number));
		}
		public static bool TryParse(string str, out LongNumber number)
		{
			try
			{ 
				number = new LongNumber(str);
				return true;
			}
			catch
			{
				number = null;
				return false;
			}
		}

		private static void Ffff(LongNumber number)
		{
			while(number.number[0] != '0')
			{
				number.number = number.number.Remove(0);
			}
		}
		private static void Two_minus_to_plus(LongNumber number)
		{
			char[] arr = new char[number.number.Length - 2];
			for (int j = 2; j < number.number.Length; j++)
			{
				arr[j - 2] = number.number[j];
			}
			number.number = new string(arr);
			return;
		}
		private static void Give_ten(LongNumber number,int i)
		{
			char[] arr = number.number.ToCharArray();
			int k = i,
				temp;
			while (true)
			{
				k--;
				if (arr[k] != '0')
				{
					temp = (arr[k] - '0');
					temp--;
					arr[k] = Convert.ToChar('0' + temp);
					k++;
					break;
				}
			}
			while(k < i)
			{
				temp = 9;
				arr[k] = Convert.ToChar('0' + temp);
				k++;
			}
			if (arr[0] == '0')
			{
				char[] arr1 = new char [arr.Length - 1];
				for (int j = 0; j < arr.Length - 1; j++)
				{
					arr1[j] = arr[j + 1];
				}
				arr = arr1;
			}
			number.number = new string(arr);
		}
		private static void Swap(LongNumber number1,LongNumber number2)
		{
			LongNumber temp = new();
			temp.number = number1.number;
			number1.number = number2.number;
			number2.number = temp.number;
			return;
		}
		private static LongNumber Mod (LongNumber number)
		{
			if (number.number[0] == '-')
			{
				LongNumber mod = new();
				char[] arr = new char[number.number.Length - 1];
				for (int i = 1; i < number.number.Length; i++)
				{
					arr[i - 1] = number.number[i];
				}
				mod.number = new string(arr);
				return mod;
			}
			return number;
		}

		public override bool Equals(object number) => this.Equals(number as LongNumber);
		public override int GetHashCode()
		{
			return number.GetHashCode();
		}
		public bool Equals(LongNumber number)
		{
			if (number is LongNumber && this.number == number.number)
			{
				return true;
			}
			return false;
		}

	}


	class Program
	{
		static void Main(string[] args)
		{
		//	int a = 24546,
		//		b = 92;
			LongNumber a1 = new("96372656257285432687319471");
			LongNumber b1 = new("43582592345635737");

			//Console.WriteLine(a + " / " + b + " = " + (a / b) + '\n');
			Console.WriteLine("LongNumber = " + (a1 / b1));

		}
	}
}