using System;
using System.Collections;
using System.Collections.Generic;

namespace Andtech.Dataspace
{

	public class DatabaseEventArgs<TKey, TValue> : EventArgs
	{
		public readonly TKey Key;
		public readonly TValue Data;

		public DatabaseEventArgs(TKey key, TValue data)
		{
			Key = key;
			Data = data;
		}
	}

	public class Database<TKey, TValue>
		: IDictionary<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

		#region INTERFACE
		public void Add(TKey key, TValue value)
		{
			dictionary.Add(key, value);

			Created?.Invoke(this, new DatabaseEventArgs<TKey, TValue>(key, value));
		}

		public bool Remove(TKey key)
		{
			if (dictionary.TryGetValue(key, out var value))
			{
				Deleted?.Invoke(this, new DatabaseEventArgs<TKey, TValue>(key, value));

				return true;
			}

			return false;
		}

		bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get => dictionary[key];
			set => dictionary[key] = value;
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => dictionary.Keys;
		ICollection<TValue> IDictionary<TKey, TValue>.Values => dictionary.Values;

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			dictionary.Add(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			dictionary.Clear();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return dictionary.Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			dictionary.CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return dictionary.Remove(item);
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count => dictionary.Count;
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => dictionary.IsReadOnly;

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}
		#endregion

		#region EVENT
		public event EventHandler<DatabaseEventArgs<TKey, TValue>> Created;
		public event EventHandler<DatabaseEventArgs<TKey, TValue>> Deleted;
		#endregion
	}
}