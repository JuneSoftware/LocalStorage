using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Logger = June.DebugLogger;

namespace June.LocalStorage.Providers {

	/// <summary>
	/// Secured JSON local store.
	/// </summary>
	public class SecuredJSONLocalStore : JSONLocalStore {

		/// <summary>
		/// The Encryption Key
		/// </summary>
		private const string ENCRYPTION_KEY = "CHANGE_THIS_KEY";

		/// <summary>
		/// Deserialize the specified data.
		/// </summary>
		/// <param name="data">Data.</param>
		protected override void Deserialize (string data) {
			base.Deserialize (XOR (ENCRYPTION_KEY, data));
		}

		/// <summary>
		/// Serialize the specified data.
		/// </summary>
		/// <param name="data">Data.</param>
		protected override string Serialize (IDictionary<string, object> data) {
			return XOR (ENCRYPTION_KEY, base.Serialize (data));
		}

		/// <summary>
		/// XOR's the specified key and input.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="input">Input.</param>
		private static string XOR(string key, string input) {
			StringBuilder output = new StringBuilder();
			for(int i=0; i < input.Length; i++) {
				output.Append((char)(input[i] ^ key[(i % key.Length)]));
			}
			return output.ToString ();
		}
	}
}