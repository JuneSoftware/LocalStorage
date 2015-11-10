using System;
using System.Collections.Generic;
using UnityEngine;

using Logger = June.DebugLogger;

namespace June.LocalStorage.Providers {

	/// <summary>
	/// Default local storage provider uses PlayerPerfs as persistent store.
	/// </summary>
	public partial class DefaultLocalStore : ILocalStore {

		private const string TYPE_INT = "int";
		private const string TYPE_FLOAT = "float";
		private const string TYPE_STRING = "string";

		#region Methods to track Keys
		/// <summary>
		/// This stores the key names and types.
		/// This is used for serialization/deserialization of the data
		/// </summary>
		protected const string _INDEX_KEY = "__KEYS";

		/// <summary>
		/// In memory representaion of the index.
		/// </summary>
		protected Dictionary<string, string> _INDEX = new Dictionary<string, string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultLocalStorage"/> class.
		/// </summary>
		public DefaultLocalStore() {
			DeSerializeKeys();
		}

		/// <summary>
		/// Adds a key to the index.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="type">Type.</param>
		protected void AddKey(string key, string type) {
			if(!_INDEX.ContainsKey(key)) {
				_INDEX.Add(key, type);
				PlayerPrefs.SetString(_INDEX_KEY, SerializeKeys());
			}
		}

		/// <summary>
		/// Removes a key from the index.
		/// </summary>
		/// <param name="key">Key.</param>
		protected void RemoveKey(string key) {
			if(_INDEX.ContainsKey(key)) {
				_INDEX.Remove(key);
				PlayerPrefs.SetString(_INDEX_KEY, SerializeKeys());
			}
		}

		/// <summary>
		/// Serializes the keys present in the in-memory representation of the index.
		/// </summary>
		/// <returns>The keys.</returns>
		protected string SerializeKeys() {
			string[] data = new string[_INDEX.Count];
			int index=0;
			foreach(var kv in _INDEX) {
				data[index++] = string.Format("{0}:{1}", kv.Key, kv.Value);
			}
			return string.Join(SEPARATOR.ToString(), data);
		}

		/// <summary>
		/// Deserialize keys from PlayerPerfs and creates a in-memory representation.
		/// </summary>
		protected void DeSerializeKeys() {
			if(PlayerPrefs.HasKey(_INDEX_KEY)) {
				string data = PlayerPrefs.GetString(_INDEX_KEY);
				string[] items = data.Split(SEPARATOR);
				foreach(string item in items) {
					string[] parts = item.Split(':');
					_INDEX.Add(parts[0], parts[1]);
				}
			}		
		}
		#endregion
		
		#region implemented abstract members of LocalStorage
		/// <summary>
		/// Gets an integer value for the key.
		/// </summary>
		/// <returns>The int value.</returns>
		/// <param name="key">Key.</param>
		public override int GetInt (string key) {
			return PlayerPrefs.GetInt(key);
		}

		/// <summary>
		/// Gets the float value for the key.
		/// </summary>
		/// <returns>The float value.</returns>
		/// <param name="key">Key.</param>
		public override float GetFloat (string key) {
			return PlayerPrefs.GetFloat(key);
		}

		/// <summary>
		/// Gets the string value for the key.
		/// </summary>
		/// <returns>The string value.</returns>
		/// <param name="key">Key.</param>
		public override string GetString (string key) {
			return PlayerPrefs.GetString(key);
		}

		/// <summary>
		/// Sets the int value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetInt (string key, int value) {
			AddKey(key, TYPE_INT);
			PlayerPrefs.SetInt(key, value);
			Save ();
		}

		/// <summary>
		/// Sets the float value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetFloat (string key, float value) {
			AddKey(key, TYPE_FLOAT);
			PlayerPrefs.SetFloat(key, value);
			Save ();
		}

		/// <summary>
		/// Sets the string value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetString (string key, string value) {
			AddKey(key, TYPE_STRING);
			SetStringWithoutKey(key, value);
			Save ();
		}

		/// <summary>
		/// Sets the string value for a key without including it in the INDEX.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void SetStringWithoutKey(string key, string value) {
			PlayerPrefs.SetString(key, value);
		}

		/// <summary>
		/// Checks if the key exists.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="key">Key.</param>
		public override bool HasKey (string key) {
			return PlayerPrefs.HasKey(key);
		}
		
		/// <summary>
		/// Saves currently modified data.
		/// </summary>
		public override void Save () {
			PlayerPrefs.Save();
		}

		/// <summary>
		/// Deletes the key from storage and INDEX.
		/// </summary>
		/// <param name="key">Key.</param>
		public override void DeleteKey (string key) {
			RemoveKey(key);
			PlayerPrefs.DeleteKey(key);
		}

		/// <summary>
		/// Clears all the data.
		/// </summary>
		public override void DeleteAll () {
			_INDEX.Clear();
			PlayerPrefs.DeleteAll();
		}

		/// <summary>
		/// Gets the serialized data.
		/// </summary>
		/// <returns>The serialized data.</returns>
		public override IDictionary<string, object> GetSerializedData () {
			var data = new Dictionary<string, object>();
			foreach(var kv in _INDEX) {
				if(this.HasKey(kv.Key)) {
					switch(kv.Value) {
						case TYPE_INT:
							data.Add(kv.Key, this.GetInt(kv.Key));
							break;
						case TYPE_FLOAT:
							data.Add(kv.Key, this.GetFloat(kv.Key));
							break;
						case TYPE_STRING:
							data.Add(kv.Key, this.GetString(kv.Key));
							break;
					}
				}
			}
			return data;
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public override void Initialize () {
			
		}
		#endregion		
	}
}
