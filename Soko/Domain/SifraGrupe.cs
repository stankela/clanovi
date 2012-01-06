using System;
using System.Text;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for SifraGrupe.
	/// </summary>
	public class SifraGrupe : IComparable
	{
		public string Value
		{
			get { return BrojGrupe.ToString() + Podgrupa; }
		}

		private int brojGrupe;
		public int BrojGrupe
		{
			get { return brojGrupe; }
            private set { brojGrupe = value; }
		}

		private string podgrupa;
		public string Podgrupa
		{
			get { return podgrupa; }
            private set { podgrupa = value; }
		}

		public SifraGrupe(string sifra)
		{
			SifraGrupe sg;
			if (!TryParse(sifra, out sg))
				throw new ArgumentException();

			this.brojGrupe = sg.BrojGrupe;
			this.podgrupa = sg.podgrupa;
		}

		protected SifraGrupe(int brojGrupe, string podgrupa)
		{
			this.brojGrupe = brojGrupe;
			this.podgrupa = podgrupa;
		}

		private SifraGrupe()
		{
		
		}

		public static SifraGrupe Parse(string s)
		{
			string msg = "Nedozvoljena vrednost za sifru grupe.";

			s = s.Trim();
			if (String.IsNullOrEmpty(s))
				throw new FormatException(msg);

			if (!startsWithDigit(s))
				throw new FormatException(msg);

			string[] parts = getParts(s);
			int broj;
			try
			{
				broj = Int32.Parse(parts[0]);
			}
			catch (Exception)
			{
				throw new FormatException(msg);
			}
			string podgrupa = parts[1].Trim();

			return new SifraGrupe(broj, podgrupa);
		}

		private static bool startsWithDigit(string s)
		{
			return Char.IsDigit(s.Trim()[0]);
		}

		private static string[] getParts(string s)
		{
			s = s.Trim();

			int i= 0;
			for (; i < s.Length; i++)
			{
				if (!Char.IsDigit(s[i]))
					break;
			}

			string digits = s.Substring(0, i);
			string rest = s.Substring(i).Trim();
			return new string[2] { digits, rest };
		}

		public static bool TryParse(string s, out SifraGrupe result)
		{
			result = new SifraGrupe();

			s = s.Trim();
			if (String.IsNullOrEmpty(s))
				return false;

			if (!startsWithDigit(s))
				return false;

			string[] parts = getParts(s);
			int broj;
			try
			{
				broj = Int32.Parse(parts[0]);
			}
			catch (Exception)
			{
				return false;
			}
			string podgrupa = parts[1].Trim();

			result = new SifraGrupe(broj, podgrupa);
			return true;
		}

		public override string ToString()
		{
			return Value;
		}

		public override bool Equals(object other)
		{
			if (object.ReferenceEquals(this, other))
				return true;
			if (!(other is SifraGrupe))
				return false;
			SifraGrupe that = (SifraGrupe)other;
			return this.BrojGrupe == that.BrojGrupe && this.Podgrupa == that.Podgrupa;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = 14;
				result = 29 * result + BrojGrupe.GetHashCode() + Podgrupa.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(SifraGrupe s1, SifraGrupe s2)
		{
			if (object.ReferenceEquals(s1, null))
				return object.ReferenceEquals(s2, null);
			else
				return s1.Equals(s2);
		}

		public static bool operator !=(SifraGrupe s1, SifraGrupe s2)
		{
			return !(s1 == s2);
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (!(obj is SifraGrupe))
				throw new ArgumentException();

			SifraGrupe other = (SifraGrupe)obj;
			int brojCompare = this.brojGrupe.CompareTo(other.brojGrupe);
			if (brojCompare != 0)
				return brojCompare;
			else
				return this.podgrupa.CompareTo(other.podgrupa);
		}

		#endregion
	}
}
