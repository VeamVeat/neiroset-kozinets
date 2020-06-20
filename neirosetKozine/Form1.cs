using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace neirosetKozine
{
	public partial class Neiroset : Form
	{
		public int sizePanel = (int)Number.Ten;
		public int sizeChar = (int)Number.Five;
		public int DownPos = (int)Number.Zero;
		public int RightPos = (int)Number.Zero;
		public float Zmin = (int)Number.Zero;
		public int PositionZminVektor = (int)Number.Zero;
		public int PositionLyamdaMaxVektor = (int)Number.Zero;
		public string CharElement = "Г";
		public string[,] ArrayCharOne; // Символ Г
		public string[,] ArrayChartwo; // Символ Ж
		public string[,] Zarray;//матрица Z
		public string[] LineZarray;//длина вектора Z
		public string[] WnullMinVectorZ; // минимальный вектор z
		public string[] LyamdaMaxVector; // максимальный вектор 
		public string[] ArrayLyamd;
		double MaxNumberLyamda = (int)Number.Zero;
		public int[] ArrayChar;// считанный массив 10x10 из файла
		public string[] W;
		public string[] CoppyZarray;

		public Neiroset()
		{
			InitializeComponent();
			for (char i = 'А'; i < 'Я'; i++)
				ComboBox.Items.Add(i + "");

			int Y = (int)Number.Zero;
			int X = (int)Number.Zero;

			for (int i = 0; i < sizePanel; i++)
			{
				for (int j = 0; j < sizePanel; j++)
				{
					CheckBox checkBox = new CheckBox();
					checkBox.Appearance = Appearance.Button;
					checkBox.Height = 23;
					checkBox.Width = 23;
					checkBox.Location = new System.Drawing.Point(X, Y);
					DataContextChar.Controls.Add(checkBox);
					X += 23;
				}
				Y += 23;
				X = (int)Number.Zero;
			}

			ArrayChar = PositionCharDataContex(DownPos, RightPos);
			DisplayCharDataContext(ArrayChar);
			KeyDown += ( s, e ) =>
			{
				if (e.KeyValue == (char)Keys.Right)
				{
					if (RightPos == (int)Number.Five)
					{
						MessageBox.Show("Вы вышли за пределы поля");
						RightPos -= (int)Number.One;
					}
					DataContextCharClear();
					ArrayChar = PositionCharDataContex(DownPos, RightPos += (int)Number.One);
					DisplayCharDataContext(ArrayChar);
				}
				if (e.KeyValue == (char)Keys.Down)
				{
					if (DownPos == (int)Number.Five)
					{
						MessageBox.Show("Вы вышли за пределы поля");
						DownPos -= (int)Number.One;
					}
					DataContextCharClear();
					ArrayChar = PositionCharDataContex(DownPos += (int)Number.One, RightPos);
					DisplayCharDataContext(ArrayChar);
				}
				if (e.KeyValue == (char)Keys.Left)
				{
					if (RightPos == (int)Number.Zero)
					{
						MessageBox.Show("Вы вышли за пределы поля");
						RightPos += (int)Number.One;
					}
					DataContextCharClear();
					ArrayChar = PositionCharDataContex(DownPos, RightPos -= (int)Number.One);
					DisplayCharDataContext(ArrayChar);
				}
				if (e.KeyValue == (char)Keys.Up)
				{
					if (DownPos == (int)Number.Zero)
					{
						MessageBox.Show("Вы вышли за пределы поля");
						DownPos += (int)Number.One;
					}
					DataContextCharClear();
					ArrayChar = PositionCharDataContex(DownPos -= (int)Number.One, RightPos);
					DisplayCharDataContext(ArrayChar);
				}
			};
		}

		private void ClearDataContext_Click( object sender, EventArgs e )
		{
			for (int i = 0; i < DataContextChar.Controls.Count; i++)
				((CheckBox)DataContextChar.Controls[i]).Checked = false;
			textBoxRezult.Clear();
		}

		//показать букву в DataContext
		private void GenerikCharButtun_Click( object sender, EventArgs e )
		{
			textBoxRezult.Focus();
			CharElement = ComboBox.Text;
			ArrayChar = PositionCharDataContex(DownPos, RightPos);
			for (int j = 0; j < ArrayChar.Length; j++)
				(DataContextChar.Controls[j] as CheckBox).Checked = Convert.ToBoolean(Convert.ToInt32(ArrayChar[j]));
		}
		//Очистка поверхности DataContext
		void DataContextCharClear()
		{
			for (int i = 0; i < sizePanel * sizePanel; i++)
				(DataContextChar.Controls[i] as CheckBox).Checked = false;
		}

		//Сдвиг буквы по матрицы 
		int[] PositionCharDataContex( int button, int right )
		{

			StreamReader streamReader = new StreamReader(@"simvol\" + CharElement + ".txt");
			int[] array = new int[100];
			string[] ArrayCharElement;
			int NumberPosition = (button * 10) + right;
			for (int i = 0; i < sizeChar; i++)
			{
				ArrayCharElement = streamReader.ReadLine().Split(new string[] { " " },
					StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < ArrayCharElement.Length; j++)
					array[NumberPosition + j] = Convert.ToInt32(ArrayCharElement[j]);
				NumberPosition += 10;
			}
			return array;
		}


		//Отображение буквы в DataContext
		void DisplayCharDataContext( int[] vs )
		{
			for (int j = 0; j < vs.Length; j++)
				(DataContextChar.Controls[j] as CheckBox).Checked = Convert.ToBoolean(Convert.ToInt32(vs[j]));
		}


		void ChangeCharButton_Click( object sender, EventArgs e )
		{
			textBoxRezult.Text = "Идёт обучение...";
			Thread thread = new Thread(PositionChars);
			thread.Start();
		}


		// Считаем все возможные позиции для двух букв
		void PositionChars()
		{
			CharElement = OneLetter.Text;
			int Z = (int)Number.Zero;
			ArrayCharOne = new string[(sizeChar + 1) * (sizeChar + 1), sizePanel * sizePanel];
			ArrayChartwo = new string[(sizeChar + 1) * (sizeChar + 1), sizePanel * sizePanel];

			for (int l = 0; l < 2; l++)
			{
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 6; j++)
					{
						ArrayChar = PositionCharDataContex(i, j);
						if (l == (int)Number.One)
						{
							for (int d = 0; d < ArrayChar.Length; d++)
								ArrayChartwo[Z, d] = ArrayChar[d].ToString();

						}
						else
						{
							for (int k = 0; k < ArrayChar.Length; k++)
								ArrayCharOne[Z, k] = ArrayChar[k].ToString();
						}
						Z++;
					}
				}
				Z = (int)Number.Zero;
				CharElement = TwoLetter.Text;
			}

			MinimumVectorZ();
			WnullMinVectorZ = new string[sizePanel * sizePanel];

			for (int l = 0; l < sizePanel * sizePanel; l++)
				WnullMinVectorZ[l] = Zarray[PositionZminVektor, l];

			for (int i = 0; i < 20000; i++)
			{
				ArrayLyamd = LyamdaVector();
				MaxNumberLyamda = MaxLyamdaVector(ArrayLyamd);
				this.Invoke(new Action(() =>
				{
					TettaTexbox.Text = "MaxLyamd =" + MaxNumberLyamda.ToString();
				}));
				if (MaxNumberLyamda > 0.001d)
					WnullMinVectorZ = SumVektor(WnullMinVectorZ,
						VektortoNumber(MaxNumberLyamda, SubtractionVector(LyamdaMaxVector, WnullMinVectorZ)));
				else
					break;
				this.Invoke(new Action(() =>
				{
					TettaTexbox.Text = "MaxLyamd = " + MaxNumberLyamda.ToString();

				}));
			}
			//Находим тэтта
			double Tetta = (Alpfa(WnullMinVectorZ) + Betta(WnullMinVectorZ)) / 2;

			StreamWriter streamWriterTetta = new StreamWriter(@"Тэтта\Т.txt");
			streamWriterTetta.Write(Tetta.ToString());
			streamWriterTetta.Close();
			StreamWriter streamWriterFinalVektor = new StreamWriter(@"FinalVektorW\FinalVektorW.txt");

			for (int i = 0; i < sizePanel * sizePanel; i++)
				streamWriterFinalVektor.WriteLine(WnullMinVectorZ[i].ToString());

			streamWriterFinalVektor.Close();
			this.Invoke(new Action(() =>
			{
				textBoxRezult.Text += Environment.NewLine;
				textBoxRezult.Text += "Обучение законченно!";

			}));
			Process.Start(@"FinalVektorW\FinalVektorW.txt");
		}


		// Считаем матрицу Z
		void MatrixZ( string[,] X, string[,] Y )
		{
			int Count = (int)Number.Zero;
			Zarray = new string[1296, sizePanel * sizePanel];
			for (int i = 0; i < (sizeChar + 1) * (sizeChar + 1); i++)
			{
				for (int j = 0; j < (sizeChar + 1) * (sizeChar + 1); j++)
				{
					for (int f = 0; f < sizePanel * sizePanel; f++)
						Zarray[Count, f] = (Convert.ToInt32(X[i, f]) - Convert.ToInt32(Y[j, f])).ToString();
					Count++;
				}
			}

		}

		//Длина каждого вектора
		void LineVektorZarray()
		{
			MatrixZ(ArrayCharOne, ArrayChartwo);
			LineZarray = new string[1296];
			int Line = (int)Number.Zero;
			for (int i = 0; i < 1296; i++)
			{
				for (int j = 0; j < sizePanel * sizePanel; j++)
					Line += (int)(Math.Pow(Convert.ToInt32(Zarray[i, j]), 2));
				LineZarray[i] = Math.Sqrt(Line).ToString();
			}
		}

		///Находим минимальное значение 
		void MinimumVectorZ()
		{
			LineVektorZarray();
			double Min = Convert.ToDouble(LineZarray[0]);

			for (int i = 1; i < LineZarray.Length; i++)
			{
				if (Convert.ToDouble(LineZarray[i]) < Min)
				{
					Min = Convert.ToDouble(LineZarray[i]);
					PositionZminVektor = i;
				}
			}

		}

		//Разница векторов
		string[] SubtractionVector( string[] A, string[] B )
		{
			string[] Vektor = new string[sizePanel * sizePanel];
			for (int i = 0; i < sizePanel * sizePanel; i++)
			{
				Vektor[i] = (Convert.ToDouble(A[i]) - Convert.ToDouble(B[i])).ToString();
			}
			return Vektor;
		}


		//Сколярное произведение векторов
		double ScolarnoeProizvedenie( string[] A, string[] B )
		{
			double RezultlNumber = (int)Number.Zero;
			for (int i = 0; i < sizePanel * sizePanel; i++) RezultlNumber +=
					(Convert.ToDouble(A[i]) * Convert.ToDouble(B[i]));
			return RezultlNumber;
		}


		//Возведение вектора в квадрат
		double PowVektor( string[] A )
		{
			double PowNumber = (int)Number.Zero;
			for (int i = 0; i < sizePanel * sizePanel; i++)
				PowNumber += Math.Pow(Convert.ToDouble(A[i]), 2);
			return PowNumber;
		}


		//Возращаем массив лямд
		string[] LyamdaVector()
		{
			string[] LyamdaArray = new string[1296];
			CoppyZarray = new string[sizePanel * sizePanel];
			for (int i = 0; i < 1296; i++)
			{
				if (i != PositionZminVektor)
				{
					for (int j = 0; j < sizePanel * sizePanel; j++)
						CoppyZarray[j] = Zarray[i, j];
					LyamdaArray[i] = ((ScolarnoeProizvedenie(WnullMinVectorZ,
										SubtractionVector(WnullMinVectorZ, CoppyZarray)))
										/ (PowVektor(SubtractionVector(WnullMinVectorZ, CoppyZarray)))).ToString();
				}

			}
			return LyamdaArray;
		}


		// Максимальное значение из массива лямд
		double MaxLyamdaVector( string[] Lyamda )
		{
			double Max = Convert.ToDouble(Lyamda[0]);
			for (int i = 1; i < Lyamda.Length; i++)
			{
				if (Convert.ToDouble(Lyamda[i]) > Max)
				{
					Max = Convert.ToDouble(Lyamda[i]);
					PositionLyamdaMaxVektor = i;
				}
			}
			LyamdaMaxVector = new string[sizePanel * sizePanel];
			for (int j = 0; j < sizePanel * sizePanel; j++)
				LyamdaMaxVector[j] = Zarray[PositionLyamdaMaxVektor, j];
			return Max;
		}


		//умножение вектора на число
		string[] VektortoNumber( double NumberLyamd, string[] Wvektor )
		{
			string[] NewVektor = new string[sizePanel * sizePanel];
			for (int i = 0; i < Wvektor.Length; i++)
				NewVektor[i] = ((Convert.ToDouble(Wvektor[i]) * NumberLyamd)).ToString();
			return NewVektor;

		}


		// Сумма векторов
		string[] SumVektor( string[] A, string[] B )
		{
			string[] NewVektor = new string[sizePanel * sizePanel];
			for (int i = 0; i < sizePanel * sizePanel; i++)
				NewVektor[i] = ((Convert.ToDouble(A[i]) + Convert.ToDouble(B[i]))).ToString();
			return NewVektor;
		}


		//Находим альфа
		double Alpfa( string[] WfinalVektor )
		{
			double[] AlphaVektor = new double[(sizeChar + 1) * (sizeChar + 1)];
			string[] CoppyArrayOnevektor = new string[sizePanel * sizePanel];
			double MinZnach;
			for (int i = 0; i < (sizeChar + 1) * (sizeChar + 1); i++)
			{
				for (int j = 0; j < sizePanel * sizePanel; j++)
					CoppyArrayOnevektor[j] = ArrayCharOne[i, j];
				AlphaVektor[0] = ScolarnoeProizvedenie(WfinalVektor, CoppyArrayOnevektor);
			}
			MinZnach = AlphaVektor[0];
			for (int j = 1; j < (sizeChar + 1) * (sizeChar + 1); j++)
				if (AlphaVektor[j] < MinZnach) MinZnach = AlphaVektor[j];

			return MinZnach;
		}


		//Находим бэтту
		double Betta( string[] WfinalVektor )
		{
			double[] AlphaVektor = new double[(sizeChar + 1) * (sizeChar + 1)];
			string[] CoppyArrayOnevektor = new string[sizePanel * sizePanel];
			double MaxZnach;
			for (int i = 0; i < (sizeChar + 1) * (sizeChar + 1); i++)
			{
				for (int j = 0; j < sizePanel * sizePanel; j++)
					CoppyArrayOnevektor[j] = ArrayChartwo[i, j];
				AlphaVektor[i] = ScolarnoeProizvedenie(WfinalVektor, CoppyArrayOnevektor);
			}
			MaxZnach = AlphaVektor[0];
			for (int j = 1; j < (sizeChar + 1) * (sizeChar + 1); j++)
				if (AlphaVektor[j] > MaxZnach) MaxZnach = AlphaVektor[j];

			return MaxZnach;
		}


		private void Raspoznanie_Click( object sender, EventArgs e )
		{
			string[] NewCharInDataContex = new string[sizePanel * sizePanel];
			string[] FinalVectorW = new string[sizePanel * sizePanel];
			double Tetta;
			int k = 9;
			textBoxRezult.Text += Environment.NewLine;
			textBoxRezult.Text = "Исходный символ:";
			textBoxRezult.Text += Environment.NewLine;
			for (int i = 0; i < sizePanel * sizePanel; i++)
			{
				if ((DataContextChar.Controls[i] as CheckBox).Checked == true)
				{
					NewCharInDataContex[i] = "1";
					textBoxRezult.Text += NewCharInDataContex[i];
				}
				else
				{
					NewCharInDataContex[i] = "0";
					textBoxRezult.Text += NewCharInDataContex[i];
				}
				if (i == k)
				{
					textBoxRezult.Text += Environment.NewLine;
					k += 10;
				}

			}
			StreamReader streamReader = new StreamReader(@"FinalVektorW\FinalVektorW.txt");
			for (int j = 0; j < sizePanel * sizePanel; j++)
				FinalVectorW[j] = streamReader.ReadLine();

			streamReader.Close();
			double Rezult = ScolarnoeProizvedenie(FinalVectorW, NewCharInDataContex);
			StreamReader str = new StreamReader(@"Тэтта\Т.txt");
			Tetta = Convert.ToDouble(str.ReadLine());
			str.Close();

			textBoxRezult.Text += Rezult;
			textBoxRezult.Text += Environment.NewLine;
			if (Rezult > Tetta) textBoxRezult.Text += "Ответ: это " + OneLetter.Text +
					" или «похоже на " + OneLetter.Text + " »";
			else textBoxRezult.Text += "Ответ: это " + TwoLetter.Text +
					" или «похоже на " + TwoLetter.Text + " »";
		}
	}

}
