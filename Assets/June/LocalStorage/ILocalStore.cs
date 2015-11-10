using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Logger = June.DebugLogger;

namespace June {
	
	/// <summary>
	/// Local storage Interface.
	/// </summary>
	public abstract partial class ILocalStore {
		
		#region Properties
		/// <summary>
		/// The default int value
		/// </summary>
		public const int DEFAULT_INT = 0;
		
		/// <summary>
		/// The default float value
		/// </summary>
		public const float DEFAULT_FLOAT = 0f;
		
		/// <summary>
		/// The default string value
		/// </summary>
		public const string DEFAULT_STRING = "";
		
		public const string TYPE_INT = "int";
		public const string TYPE_FLOAT = "float";
		public const string TYPE_STRING = "string";
		
		/// <summary>
		/// Gets the SEPARATOR used to separate individual array items.
		/// This field can be overriden by the providers if necessary.
		/// </summary>
		/// <value>
		/// The SEPARATOR character.
		/// </value>
		public virtual char SEPARATOR {
			get {
				return ',';
			}
		}
		#endregion

		#region Get Methods
		
		/// <summary>
		/// Gets the int value for a key.
		/// </summary>
		/// <returns>
		/// The int value.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public abstract int GetInt (string key);
		
		/// <summary>
		/// Gets the float value for a key.
		/// </summary>
		/// <returns>
		/// The float value.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public abstract float GetFloat (string key);
		
		/// <summary>
		/// Gets the string value for a key.
		/// </summary>
		/// <returns>
		/// The string value.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public abstract string GetString (string key);
		
		/// <summary>
		/// Gets the string array for a key.
		/// </summary>
		/// <returns>
		/// The string array.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public virtual string[] GetStringArray (string key) {
			string result = GetString (key);
			return !string.IsNullOrEmpty (result) ? result.Split (SEPARATOR) : null;
		}
		
		/// <summary>
		/// Gets the boolean value for a key.
		/// </summary>
		/// <returns>
		/// The bool.
		/// </returns>
		/// <param name='key'>
		/// If set to <c>true</c> key.
		/// </param>
		public virtual bool GetBool (string key) {
			return 1 == GetInt (key);
		}
		
		/// <summary>
		/// Gets the int value if key is present else returns default int value.
		/// </summary>
		/// <returns>
		/// The int value or default.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public virtual int GetIntOrDefault (string key) {
			return HasKey (key) ? GetInt (key) : DEFAULT_INT;
		}
		
		/// <summary>
		/// Gets the float if key is present else returns default float value.
		/// </summary>
		/// <returns>
		/// The float value or default.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public virtual float GetFloatOrDefault (string key) {
			return HasKey (key) ? GetFloat (key) : DEFAULT_FLOAT;
		}
		
		/// <summary>
		/// Gets the string value if key is present else returns default string value.
		/// </summary>
		/// <returns>
		/// The string or default.
		/// </returns>
		/// <param name='key'>
		/// Key.
		/// </param>
		public virtual string GetStringOrDefault (string key) {
			return HasKey (key) ? GetString (key) : DEFAULT_STRING;
		}
		
		/// <summary>
		/// Gets the bool value if key is present else returns default bool value.
		/// </summary>
		/// <returns>
		/// The bool or default.
		/// </returns>
		/// <param name='key'>
		/// If set to <c>true</c> key.
		/// </param>
		public virtual bool GetBoolOrDefault (string key) {
			return HasKey (key) ? 1 == GetInt (key) : false;
		}
		
		#endregion
		
		#region Set Methods
		
		/// <summary>
		/// Sets the int value for a key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public abstract void SetInt (string key, int value);
		
		/// <summary>
		/// Sets the float value for a key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public abstract void SetFloat (string key, float value);
		
		/// <summary>
		/// Sets the string value for a key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public abstract void SetString (string key, string value);
		
		/// <summary>
		/// Sets the string array value for a key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='values'>
		/// Values.
		/// </param>
		public virtual void SetStringArray (string key, string[] values) {
			SetString (key, string.Join (SEPARATOR.ToString (), values));
		}
		
		/// <summary>
		/// Sets the bool value for a key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		public virtual void SetBool (string key, bool value) {
			SetInt (key, value ? 1 : 0);
		}
		
		/// <summary>
		/// Increments the integer value of a specified key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public virtual void Increment (string key) {
			SetInt (key, GetIntOrDefault (key) + 1);
		}
		
		/// <summary>
		/// Decrement the the integer value of a specified key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public virtual void Decrement (string key) {
			SetInt (key, GetIntOrDefault (key) - 1);
		}
		
		#endregion
		
		#region Other Methods
		
		/// <summary>
		/// Initialize this instance.
		/// This is where the provider should write its initialization code.
		/// This could be called from the constructor as well. While creating an instance the LocalStorage class does not 
		/// explicity call this Initialize method, but expects the provider to be initialized after calling the constructor.
		/// 
		/// This method is only provided incase the game wants to re-initialize the provider for any reason.
		/// </summary>
		public abstract void Initialize ();
		
		/// <summary>
		/// Checks if the key exists.
		/// </summary>
		/// <returns>
		/// The key.
		/// </returns>
		/// <param name='key'>
		/// If set to <c>true</c> key.
		/// </param>
		public abstract bool HasKey (string key);
		
		/// <summary>
		/// Saves currently modified data.
		/// </summary>
		public abstract void Save ();
		
		/// <summary>
		/// Deletes the key.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public abstract void DeleteKey (string key);
		
		/// <summary>
		/// Clears all data.
		/// </summary>
		public abstract void DeleteAll ();
		
		/// <summary>
		/// Gets the serialized data.
		/// </summary>
		/// <returns>
		/// The serialized data.
		/// </returns>
		public abstract IDictionary<string, object> GetSerializedData ();
		
		/// <summary>
		/// Gets the serialized data JSON.
		/// </summary>
		/// <returns>The serialized data JSO.</returns>
		public string GetSerializedDataJSON () {
			return GetSerializedDataJSON (GetSerializedData ());
		}
		
		/// <summary>
		/// Gets the serialized data in JSON format.
		/// </summary>
		/// <returns>
		/// The serialized data JSO.
		/// </returns>
		public string GetSerializedDataJSON (IDictionary<string, object> dict) {
			StringBuilder str = new StringBuilder ();
			str.Append ("{ ");
			bool addComma = false;
			foreach (var kv in dict) {
				string format = string.Empty;
				if (kv.Value is int) {
					format = "\"{0}\":{1}";
				} else if (kv.Value is float) {
					format = "\"{0}\":{1}";
				} else {
					format = "\"{0}\":\"{1}\"";
				}
				if (addComma) {
					str.Append (", ");
				}
				str.AppendFormat (format, kv.Key, kv.Value);
				addComma = true;
			}
			str.Append (" }");
			return str.ToString ();
		}
		
		/// <summary>
		/// Deserializes the data.
		/// </summary>
		/// <returns>
		/// The data.
		/// </returns>
		/// <param name='jsonObj'>
		/// If set to <c>true</c> json object.
		/// </param>
		public virtual bool DeserializeData (IDictionary<string, object> jsonObj) {
			Logger.Log ("[LocalStorage] DeserializeData - " + jsonObj.Count);
			int intValue = 0;
			float floatValue = 0;
			double doubleValue = 0;
			foreach (var kv in jsonObj) {
				//if(false == Array.Exists(LocalStorageKeys.MIGRATION_KEYS_TO_IGNORE, k => k == kv.Key)) {
				string value = kv.Value.ToString ();
				if (int.TryParse (value, out intValue)) {
					SetInt (kv.Key, intValue);
				} else if (float.TryParse (value, out floatValue)) {
					SetFloat (kv.Key, floatValue);
				} else if (double.TryParse (value, out doubleValue)) {
					SetFloat (kv.Key, (float)doubleValue);
				} else {
					SetString (kv.Key, value);
				}
				//}
			}
			Logger.Log ("[LocalStorage] DeserializeData Complete");
			return true;
		}
		#endregion
	}
}