using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Logger = June.DebugLogger;

namespace June.LocalStorage.Providers {

	/// <summary>
	/// JSON local storage provider.
	/// </summary>
	public partial class JSONLocalStore : ILocalStore {

		protected static readonly string FILE_PATH = Application.persistentDataPath + "/LocalStorage.json";

		protected static IDictionary<string, object> DATA = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="JSONLocalStorage"/> class.
		/// </summary>
		public JSONLocalStore() {
			Initialize();
		}

		#region File IO Methods
		/// <summary>
		/// Reads the data from file.
		/// </summary>
		protected virtual void ReadDataFromFile() {
			string data = null;
			try {
				if(File.Exists(FILE_PATH)) {
					Logger.Log("[JSONLocalStorage] Opening: " + FILE_PATH);
					using(StreamReader sr = new StreamReader(FILE_PATH)) {
						data = sr.ReadToEnd();
					}
				}
			}
			catch(Exception ex) {
				Logger.LogError("[JSONLocalStorage] Error in ReadDataFromFile - " + ex.Message);
			}
			Deserialize(data);
		}

		/// <summary>
		/// Deserialize the specified data.
		/// </summary>
		/// <param name="data">Data.</param>
		protected virtual void Deserialize(string data) {
			if(!string.IsNullOrEmpty(data)) {
				object jsonObj = null;
				SimpleJson.SimpleJson.TryDeserializeObject(data, out jsonObj);
				DATA = jsonObj as IDictionary<string, object>;
			}
			else {
				DATA = new Dictionary<string, object>();
			}
		}

		/// <summary>
		/// Writes the data to file.
		/// </summary>
		protected virtual void WriteDataToFile() {
			string data = Serialize(DATA);
			if(null != data) {
				try {
					using(StreamWriter sw = new StreamWriter(FILE_PATH)) {
						sw.Write(data);
					}
				}
				catch(Exception ex) {
					Logger.LogError("[JSONLocalStorage] Error in WriteDataToFile - " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Serialize the specified data.
		/// </summary>
		/// <param name="data">Data.</param>
		protected virtual string Serialize(IDictionary<string, object> data) {
			string json = null;
			if(null != data) {
				json = SimpleJson.SimpleJson.SerializeObject(data);
			}
			return json;
		}

		#endregion
		
		#region implemented abstract members of LocalStorage
		/// <summary>
		/// Initialize this instance.
		/// This is where the provider should write its initialization code.
		/// This could be called from the constructor as well. While creating an instance the LocalStorage class does not 
		/// explicity call this Initialize method, but expects the provider to be initialized after calling the constructor.
		/// 
		/// This method is only provided incase the game wants to re-initialize the provider for any reason.
		/// </summary>
		public override void Initialize () {
			ReadDataFromFile();
			if(null == DATA) {
				DATA = new Dictionary<string, object>();
			}
		}

		/// <summary>
		/// Get the specified key's value.
		/// </summary>
		/// <param name="key">Key.</param>
		protected virtual object Get(string key) {
			object result = null;
			if(null != DATA && DATA.ContainsKey(key)) {
				result = DATA[key];
			}
			return result;
		}

		/// <summary>
		/// Gets the int value for a key.
		/// </summary>
		/// <returns>The int value.</returns>
		/// <param name="key">Key.</param>
		public override int GetInt (string key) {
			object obj = Get (key);
			return (null != obj) ? Convert.ToInt32(obj) : 0;
		}

		/// <summary>
		/// Gets the float value for a key.
		/// </summary>
		/// <returns>The float value.</returns>
		/// <param name="key">Key.</param>
		public override float GetFloat (string key) {
			object obj = Get (key);
			return (null != obj) ? Convert.ToSingle(obj) : 0f;
		}

		/// <summary>
		/// Gets the string value for a key.
		/// </summary>
		/// <returns>The string value.</returns>
		/// <param name="key">Key.</param>
		public override string GetString (string key) {
			object obj = Get (key);
			return (null != obj) ? Convert.ToString(obj) : null;
		}

		/// <summary>
		/// Set the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		protected void Set(string key, object value) {
			if(null != DATA) {
				if(DATA.ContainsKey(key)) {
					DATA[key] = value;
				}
				else {
					DATA.Add(key, value);
				}
				Save();
			}
		}

		/// <summary>
		/// Sets the int value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetInt (string key, int value) {
			Set (key, value);
		}

		/// <summary>
		/// Sets the float value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetFloat (string key, float value) {
			Set (key, value);
		}

		/// <summary>
		/// Sets the string value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetString (string key, string value) {
			Set (key, value);
		}

		/// <summary>
		/// Checks if the key exists.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="key">Key.</param>
		public override bool HasKey (string key) {
			return null != DATA ? DATA.ContainsKey(key) : false;
		}

		/// <summary>
		/// Saves currently modified data.
		/// </summary>
		public override void Save () {
			WriteDataToFile();
		}

		/// <summary>
		/// Deletes the key.
		/// </summary>
		/// <param name="key">Key.</param>
		public override void DeleteKey (string key) {
			if(null != DATA && DATA.ContainsKey(key)) {
				DATA.Remove(key);
				Save ();
			}
		}

		/// <summary>
		/// Clears all data.
		/// </summary>
		public override void DeleteAll () {
			if(null != DATA) {
				DATA.Clear();
				Save ();
			}
		}

		/// <summary>
		/// Gets the serialized data.
		/// </summary>
		/// <returns>The serialized data.</returns>
		public override System.Collections.Generic.IDictionary<string, object> GetSerializedData () {
			var dictionary = new Dictionary<string, object>();
			foreach(var kv in DATA) {
				dictionary.Add(kv.Key, kv.Value);
			}
			return dictionary;
		}

		#endregion
		
	}
}