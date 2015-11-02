using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace JuneTools { 

	public enum ValueType {
		Int,
		Float,
		String
	}

	public class LocalStorageValue {

		private string _Name;
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return _Name;
			}
			set {
				_Name = value;
			}
		}

		private object _InitialValue;
		private object _Value;
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public object Value {
			get {
				return _Value;
			}
			set {
				if(_InitialValue == null && value != null) {
					HasChanged = true;
					_Value = value;
				}
				else if(_InitialValue.ToString() != value.ToString()) {
					HasChanged = true;
					_Value = value;
				}
			}
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public ValueType Type {
			get {
				return GetValueType (Value);
			}
			set {
				if(value != this.Type) {
					switch(value) {
						case ValueType.Float:
							this.Value = 0f;
							break;
						case ValueType.Int:
							this.Value = 0;
							break;
						case ValueType.String:
							this.Value = string.Empty;
							break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the int value.
		/// </summary>
		/// <value>The int value.</value>
		public int IntValue {
			get {
				return GetIntValue (Value);
			}
			set {
				this.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the float value.
		/// </summary>
		/// <value>The float value.</value>
		public float FloatValue {
			get { 
				return GetFloatValue (Value);
			}
			set {
				this.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the string value.
		/// </summary>
		/// <value>The string value.</value>
		public string StringValue {
			get {
				return GetStrValue (Value);
			}
			set {
				this.Value = value;
			}
		}

		/// <summary>
		/// The is to be deleted.
		/// </summary>
		public bool IsToBeDeleted;

		/// <summary>
		/// The has changed.
		/// </summary>
		public bool HasChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStorageValue"/> class.
		/// </summary>
		/// <param name="keyValue">Key value.</param>
		public LocalStorageValue (KeyValuePair<string, object> keyValue) : this(keyValue.Key, keyValue.Value) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JuneTools.LocalStorageValue"/> class.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public LocalStorageValue(string key, object value) {
			this._Name = key;

			if(value is long) {
				value = Convert.ToInt32(value);
			}
			if(value is double) {
				value = Convert.ToSingle(value);
			}
			this._InitialValue = value;
			this._Value = value;

			this.IsToBeDeleted = false;
			this.HasChanged = false;
		}

		/// <summary>
		/// Gets the type of the value.
		/// </summary>
		/// <returns>The value type.</returns>
		/// <param name="value">Value.</param>
		private ValueType GetValueType (object value) {
			if (null != value) {
				Type type = value.GetType ();
				if (typeof(int) == type || typeof(long) == type) {
					return ValueType.Int;
				}

				if (typeof(float) == type || typeof(double) == type) {
					return ValueType.Float;
				}
			}

			return ValueType.String;
		}

		/// <summary>
		/// Gets the int value.
		/// </summary>
		/// <returns>The int value.</returns>
		/// <param name="value">Value.</param>
		private int GetIntValue (object value) {
			int result = 0;
			if (null != value) {
				int.TryParse (value.ToString (), out result);
			}
			return result;
		}

		/// <summary>
		/// Gets the float value.
		/// </summary>
		/// <returns>The float value.</returns>
		/// <param name="value">Value.</param>
		private float GetFloatValue (object value) {
			float result = 0f;
			if (null != value) {
				float.TryParse (value.ToString (), out result);
			}
			return result;
		}

		/// <summary>
		/// Gets the string value.
		/// </summary>
		/// <returns>The string value.</returns>
		/// <param name="value">Value.</param>
		private string GetStrValue (object value) {
			return (null != value) ? value.ToString () : string.Empty;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="JuneTools.LocalStorageValue"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="JuneTools.LocalStorageValue"/>.</returns>
		public override string ToString () {
			return string.Format ("[LocalStorageValue: Name={0}, Value={1}, Type={2}, IntValue={3}, FloatValue={4}, StringValue={5}]", Name, Value, Type, IntValue, FloatValue, StringValue);
		}

		/// <summary>
		/// Gets the values from dictionary.
		/// </summary>
		/// <returns>The values from dictionary.</returns>
		/// <param name="dictionary">Dictionary.</param>
		public static List<LocalStorageValue> GetValuesFromDictionary(IDictionary<string, object> dictionary) {
			List<LocalStorageValue> values = new List<LocalStorageValue>();
			if(null != dictionary) {
				foreach(var kv in dictionary) {
					values.Add(new LocalStorageValue(kv));
				}
			}
			return values;
		}
	}
}
