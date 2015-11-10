using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Logger = June.DebugLogger;

namespace June {

	/// <summary>
	/// Local storage manager.
	/// </summary>
	public static partial class LocalStore {

		private static ILocalStore _Instance = null;
		/// <summary>
		/// Gets the local storage instance.
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static ILocalStore Instance {
			get {
				if (null == _Instance) {
					_Instance = InitializeProvider ();
				}
				return _Instance;
			}
		}
	
		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <returns>The provider.</returns>
		public static ILocalStore InitializeProvider () {
			//This provider uses the PlayerPerfs present in Unity3d as the persistent store
			return new LocalStorage.Providers.DefaultLocalStore ();
			
			////This provider uses a JSON file as the persistent store
			//return new LocalStorage.Providers.JSONLocalStore();
			
			////This provider uses a XML file as the persistent store
			//return new LocalStorage.Providers.XMLLocalStore();

			////This provider uses a JSON file as the persistent store but also encrypts the data.
			//return new LocalStorage.Providers.SecuredJSONLocalStore();
		}
	}
}