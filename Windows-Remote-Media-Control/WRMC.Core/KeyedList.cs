using System.Collections.Generic;
using System.Linq;

namespace WRMC.Core {
	public class KeyedList<TKey, TValue> : List<KeyValuePair<TKey, TValue>> {
		public List<TKey> Keys => this.Select(kvp => kvp.Key).ToList();
		public List<TValue> Values => this.Select(kvp => kvp.Value).ToList();

		public TValue this[TKey key] {
			get => this.FirstOrDefault(kvp => EqualityComparer<TKey>.Default.Equals(key, kvp.Key)).Value;
			set {
				var element = this.Select((kvp, i) => new { kvp, i }).FirstOrDefault(e => EqualityComparer<TKey>.Default.Equals(key, e.kvp.Key));

				if (element == null || element.i < 0)
					this.Add(new KeyValuePair<TKey, TValue>(key, value));
				else {
					this.RemoveAt(element.i);

					if (element.i >= this.Count)
						this.Add(new KeyValuePair<TKey, TValue>(key, value));
					else
						this.Insert(element.i, new KeyValuePair<TKey, TValue>(key, value));
				}
			}
		}

		public bool ContainsKey(TKey key) => this.Where(kvp => EqualityComparer<TKey>.Default.Equals(key, kvp.Key)).Count() > 0;
		public bool ContainsValue(TValue value) => this.Where(kvp => EqualityComparer<TValue>.Default.Equals(value, kvp.Value)).Count() > 0;

		public void Add(TKey key, TValue value) {
			this[key] = value;
		}

		public void MoveToEnd(TKey key) {
			var element = this.Select((kvp, i) => new { kvp, i }).FirstOrDefault(e => EqualityComparer<TKey>.Default.Equals(key, e.kvp.Key));

			if (element == null || element.i < 0)
				return;

			this.RemoveAt(element.i);
			this.Add(new KeyValuePair<TKey, TValue>(element.kvp.Key, element.kvp.Value));
		}
	}
}