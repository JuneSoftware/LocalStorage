using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using Logger = June.DebugLogger;

namespace June.LocalStorage.Providers {

	/// <summary>
	/// XML local storage.
	/// </summary>
	public partial class XMLLocalStore : ILocalStore {

		protected static readonly string FILE_PATH = Application.persistentDataPath + "/LocalStorage.xml";

		protected static XmlStore _XML_STORE = null;


		/// <summary>
		/// Initializes a new instance of the <see cref="XMLLocalStorage"/> class.
		/// </summary>
		public XMLLocalStore() {
			Initialize();
		}

		
		#region File IO Methods
		/// <summary>
		/// Reads the data from file.
		/// </summary>
		protected virtual void ReadDataFromFile() {
			if(File.Exists(FILE_PATH)) {
				using (FileStream file = new FileStream(FILE_PATH, FileMode.OpenOrCreate)) {
					if(file.Length > 0) {
						XmlSerializer serializer = new XmlSerializer (typeof(XmlStore));
						_XML_STORE = (XmlStore)serializer.Deserialize (file);
					}
				}
			}

			if(null == _XML_STORE) {
				_XML_STORE = new XmlStore();
			}
		}

		/// <summary>
		/// Writes the data to file.
		/// </summary>
		protected virtual void WriteDataToFile() {
			using (FileStream file = new FileStream(FILE_PATH, FileMode.Create)) {
				XmlSerializer serializer = new XmlSerializer (typeof(XmlStore));
				serializer.Serialize (file, _XML_STORE);
			}
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
		}

		/// <summary>
		/// Gets the int value for a key.
		/// </summary>
		/// <returns>The int value.</returns>
		/// <param name="key">Key.</param>
		public override int GetInt (string key) {
			return (int)_XML_STORE[key].Value;
		}

		/// <summary>
		/// Gets the float value for a key.
		/// </summary>
		/// <returns>The float value.</returns>
		/// <param name="key">Key.</param>
		public override float GetFloat (string key) {
			return (float)_XML_STORE[key].Value;
		}

		/// <summary>
		/// Gets the string value for a key.
		/// </summary>
		/// <returns>The string value.</returns>
		/// <param name="key">Key.</param>
		public override string GetString (string key) {
			return _XML_STORE[key].Value as string;
		}

		/// <summary>
		/// Gets the item or create default.
		/// </summary>
		/// <returns>The item or create default.</returns>
		/// <param name="key">Key.</param>
		protected XmlItem GetItemOrCreateDefault(string key) {
			var item = _XML_STORE[key];
			if(null == item) {
				item = new XmlItem();
				item.Key = key;
				_XML_STORE.Items.Add(item);
			}
			return item;
		}

		/// <summary>
		/// Sets the int value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetInt (string key, int value) {
			var item = GetItemOrCreateDefault(key);
			item.ValueStr = value.ToString();
			item.Type = TYPE_INT;
			Save ();
		}

		/// <summary>
		/// Sets the float value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetFloat (string key, float value) {
			var item = GetItemOrCreateDefault(key);
			item.ValueStr = value.ToString();
			item.Type = TYPE_FLOAT;
			Save ();
		}

		/// <summary>
		/// Sets the string value for a key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public override void SetString (string key, string value) {
			var item = GetItemOrCreateDefault(key);
			item.ValueStr = value;
			item.Type = TYPE_STRING;
			Save ();
		}

		/// <summary>
		/// Checks if the key exists.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="key">Key.</param>
		public override bool HasKey (string key) {
			return _XML_STORE.ContainsKey(key);
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
			var item = _XML_STORE[key];
			if(null != item) {
				_XML_STORE.Items.Remove(item);
			}
			Save();
		}

		/// <summary>
		/// Clears all data.
		/// </summary>
		public override void DeleteAll () {
			_XML_STORE.Items.Clear();
			Save ();
		}

		/// <summary>
		/// Gets the serialized data.
		/// </summary>
		/// <returns>The serialized data.</returns>
		public override System.Collections.Generic.IDictionary<string, object> GetSerializedData () {
			return _XML_STORE.GetDictionary();
		}

		#endregion

		#region XML Serializer Classes

		///<summary>
		/// Xml Store Class
		/// </summary>
		public class XmlStore {
			/// <summary>
			/// The items.
			/// </summary>
			[XmlArray("items")]
			public List<XmlItem> Items;

			/// <summary>
			/// Gets the <see cref="June.LocalStorageProviders.XMLLocalStorage+XmlStore"/> with the specified key.
			/// </summary>
			/// <param name="key">Key.</param>
			public XmlItem this[string key] {
				get {
					return GetItemByKey(key);
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="June.LocalStorageProviders.XMLLocalStorage+XmlStore"/> class.
			/// </summary>
			public XmlStore() {
				this.Items = new List<XmlItem>();
			}

			/// <summary>
			/// Gets the dictionary.
			/// </summary>
			/// <returns>The dictionary.</returns>
			public IDictionary<string, object> GetDictionary() {
				Dictionary<string, object> data = new Dictionary<string, object>();
				foreach(var item in Items) {
					if(null != item && false == data.ContainsKey(item.Key)) {
						data.Add(item.Key, item.Value);
					}
				}
				return data;
			}

			/// <summary>
			/// Contains the key.
			/// </summary>
			/// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
			/// <param name="key">Key.</param>
			public bool ContainsKey(string key) {
				return null != Items.FirstOrDefault(x => x.Key == key);
			}

			/// <summary>
			/// Gets the item by key.
			/// </summary>
			/// <returns>The item by key.</returns>
			/// <param name="key">Key.</param>
			public XmlItem GetItemByKey(string key) {
				return Items.FirstOrDefault(x => x.Key == key);
			}
		}
		
		/// <summary>
		/// Xml Store Item Class
		/// </summary>
		public class XmlItem {
			/// <summary>
			/// The key.
			/// </summary>
			[XmlAttribute("key")]
			public string Key;

			/// <summary>
			/// The value string.
			/// </summary>
			[XmlAttribute("value")]
			public string ValueStr;

			/// <summary>
			/// The type.
			/// </summary>
			[XmlAttribute("type")]
			public string Type;

			/// <summary>
			/// Gets the value.
			/// </summary>
			/// <value>The value.</value>
			public object Value {
				get {
					object obj = null;
					switch(this.Type) {
					case TYPE_INT:
						int intValue = 0;
						if(!string.IsNullOrEmpty(this.ValueStr) && int.TryParse(this.ValueStr, out intValue)) {
							obj = intValue;
						}
						break;
					case TYPE_FLOAT:
						float floatValue = 0f;
						if(!string.IsNullOrEmpty(this.ValueStr) && float.TryParse(this.ValueStr, out floatValue)) {
							obj = floatValue;
						}
						break;
					case TYPE_STRING:
						obj = ValueStr;
						break;
					default:
						break;
					}
					return obj;
				}
			}
		}

		#endregion
	}
}