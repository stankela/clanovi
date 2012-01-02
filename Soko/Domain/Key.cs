using System;

namespace Soko.Domain
{
	public class Key
	{
		private object[] fields;

		public Key(object[] fields) 
		{
			checkKeyNotNull(fields);
			this.fields = fields;
		}

		private void checkKeyNotNull(object[] fields) 
		{
			if (fields == null)
				throw new ArgumentException("Cannot have a null key");
			if (fields.Length == 0)
				throw new ArgumentException("Cannot have an empty key");
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i] == null)
					throw new ArgumentException("Cannot have a null element of key");
			}
		}
		
		// Convenient constructors

		public Key(int arg) 
		{
			this.fields = new object[1];
			this.fields[0] = arg;
		}
		
		public Key(string arg) 
		{
			this.fields = new object[1];
			this.fields[0] = arg;
		}
		
		public Key(object field) 
		{
			if (field == null)
				throw new ArgumentException("Cannot have a null key");
			this.fields = new object[1];
			this.fields[0] = field;
		}

		public Key(object arg1, object arg2) 
		{
			this.fields = new object[2];
			this.fields[0] = arg1;
			this.fields[1] = arg2;
			checkKeyNotNull(fields);
		}

		// accessor functions to get parts of key

		public object Value(int i) 
		{
			checkRange(i);
			return fields[i];
		}

		private void checkRange(int i)
		{
			if (i < 0 || i > fields.Length - 1)
				throw new ArgumentException("Index out of range");
		}
		
		public object Value() 
		{
			checkSingleKey();
			return fields[0];
		}
		
		private void checkSingleKey() 
		{
			if (fields.Length > 1)
				throw new InvalidOperationException("Cannot take value on composite key");
		}
		
		public int intValue() 
		{
			checkSingleKey();
			return intValue(0);
		}
		
		public int intValue(int i) 
		{
			checkRange(i);
			if (fields[i].GetType() != typeof(int))
				throw new InvalidOperationException("Cannot take intValue on non int key");
			return Convert.ToInt32(fields[i]);
		}
		
		public string stringValue() 
		{
			checkSingleKey();
			return stringValue(0);
		}
		
		public string stringValue(int i) 
		{
			checkRange(i);
			if (fields[i].GetType() != typeof(string))
				throw new InvalidOperationException("Cannot take stringValue on non string key");
			return fields[i].ToString();
		}
		
		public override bool Equals(object obj) 
		{
			if (object.ReferenceEquals(this, obj)) return true;
			if (!(obj is Key)) return false;
			Key other = (Key)obj;
			if (this.fields.Length != other.fields.Length) return false;
			for (int i = 0; i < fields.Length; i++)
			{
				if (!this.fields[i].Equals(other.fields[i])) return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = 14;
				for (int i = 0; i < fields.Length; i++)
				{
					result = 29 * result + fields[i].GetHashCode();
				}
				return result;
			}

		}

		public static bool operator ==(Key k1, Key k2)
		{
			if (object.ReferenceEquals(k1, null))
				return object.ReferenceEquals(k2, null);
			else
				return k1.Equals(k2);
		}

		public static bool operator !=(Key k1, Key k2)
		{
			return !(k1 == k2);
		}

	}
}
