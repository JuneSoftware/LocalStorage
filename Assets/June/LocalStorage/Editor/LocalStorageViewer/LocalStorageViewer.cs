using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using June;

using Logger = UnityEngine.Debug;

namespace JuneTools {

	/// <summary>
	/// Local storage editor window.
	/// </summary>
	public class LocalStorageViewer : EditorWindow {
	
		public List<LocalStorageValue> VALUES = null;
		private Vector2 _ScrollPosition;
		private bool _IsCreatingNew;
		private LocalStorageValue _NewValue;

		#region UI Methods
		private GUIStyle _BoxStyle;
		/// <summary>
		/// Gets the box style.
		/// </summary>
		/// <value>The box style.</value>
		private GUIStyle BoxStyle {
			get {
				if (_BoxStyle == null) {
					GUIStyleState state = new GUIStyleState ();
					state.background = MakeBoxBGTexture ();

					_BoxStyle = new GUIStyle () {
					normal = state,
					border = new RectOffset(6, 6, 6, 6),
					margin = new RectOffset(4, 4, -1, -1),
					padding = new RectOffset(10, 6, 6, 6),
					stretchHeight = true,
					stretchWidth = true
				};
				}
				return _BoxStyle;
			}
		}

		/// <summary>
		/// Makes the box background texture.
		/// </summary>
		/// <returns>The box background texture.</returns>
		private Texture2D MakeBoxBGTexture () {
			Color light = new Color (0.812f, 0.812f, 0.812f, 0.153f);
			Color dark = new Color (0f, 0f, 0f, 0.090f);
			Texture2D texture = new Texture2D (8, 8, TextureFormat.ARGB32, false);
			for (int x = 0; x < 8; x++) {
				for (int y = 0; y < 8; y++) {
					if (x == 0 || y == 0 || x == 7 || y == 7) {
						texture.SetPixel (x, y, light);
					}
					else {
						texture.SetPixel (x, y, dark);
					}
				}
			}
			texture.Apply ();
			return texture;
		}
		#endregion

		[MenuItem ("June/Local Storage Viewer")]	
		private static void Init() {
			GetWindow<LocalStorageViewer>("LocalStorageView");
		}

		private void OnGUI () {

			// Sanity Checks
			if (null == this.VALUES) { 
				RefreshData ();
			}

			// Toolbar
			RenderToolbar ();

			// Create New
			if (this._IsCreatingNew) {
				RenderCreateNew ();	
			}
		
			// Displays current local store provider type.
			GUILayout.Label ("Local Storage Values for : " + LocalStore.Instance.ToString ());
		
			// Render stored values
			RenderValues ();
		}

		/// <summary>
		/// Renders the toolbar.
		/// </summary>
		private void RenderToolbar () {
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			if (GUILayout.Button ("Create New", EditorStyles.toolbarButton)) {
				this._NewValue = new LocalStorageValue (new KeyValuePair<string, object> (string.Empty, 0));
				this._IsCreatingNew = true;
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Print", EditorStyles.toolbarButton)) {
				DebugPrint (this.VALUES);
			}
			if (GUILayout.Button ("Save All", EditorStyles.toolbarButton)) {
				SaveAll ();
				RefreshData ();
			}
			if (GUILayout.Button ("Refresh/Restore", EditorStyles.toolbarButton)) {
				RefreshData ();
			}
			if (GUILayout.Button ("DELETE ALL DATA", EditorStyles.toolbarButton)) {
				if (EditorUtility.DisplayDialog ("Delete All Data", "Are you sure you want to delete all the data?", "YES", "NO")) {
					LocalStore.Instance.DeleteAll ();
					RefreshData ();
				}
			}
			GUILayout.EndHorizontal ();
		}

		/// <summary>
		/// Renders the create new section.
		/// </summary>
		void RenderCreateNew () {
			if (null == this._NewValue) {
				this._NewValue = new LocalStorageValue (new KeyValuePair<string, object> (string.Empty, 0f));
			}
			GUILayout.BeginArea (new Rect (5, 20, position.width - 10, 98), BoxStyle);
			GUILayout.Space (3);
			GUILayout.Label ("Create New", EditorStyles.boldLabel);
			this._NewValue.Name = EditorGUILayout.TextField ("Key : ", this._NewValue.Name);
			GUILayout.BeginHorizontal ();
			switch (this._NewValue.Type) {
			case ValueType.Int:
				this._NewValue.Value = EditorGUILayout.IntField ("Initial Value : ", this._NewValue.IntValue);
				break;
			case ValueType.Float:
				this._NewValue.Value = EditorGUILayout.FloatField ("Initial Value : ", this._NewValue.FloatValue);
				break;
			case ValueType.String:
				this._NewValue.Value = EditorGUILayout.TextField ("Initial Value : ", this._NewValue.StringValue);
				break;
			}
			ValueType valType = (ValueType)EditorGUILayout.EnumPopup (this._NewValue.Type, GUILayout.MaxWidth (80));
			if (this._NewValue.Type != valType) {
				Logger.Log ("SETTING FROM:" + this._NewValue.Type + " TO:" + valType);
				string name = this._NewValue.Name;
				object obj = string.Empty;
				if (valType == ValueType.Int) {
					obj = 0;
				}
				else
					if (valType == ValueType.Float) {
						obj = 0f;
					}
				this._NewValue = new LocalStorageValue (name, obj);
			}
			this._NewValue.Type = valType;
			GUILayout.EndHorizontal ();
			GUILayout.Space (4);
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Create")) {
				if (null == this.VALUES) {
					this.VALUES = new List<LocalStorageValue> ();
				}
				this.VALUES.Add (this._NewValue);
				this.SaveAll ();
				this._IsCreatingNew = false;
			}
			if (GUILayout.Button ("Cancel")) {
				this._IsCreatingNew = false;
				this._NewValue = null;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndArea ();
			GUILayout.Space (104);
		}

		void RenderValues ()
		{
			this._ScrollPosition = GUILayout.BeginScrollView (this._ScrollPosition);
			if (this.VALUES.Count == 0) {
				GUILayout.Label ("No local storage values for current project", EditorStyles.miniLabel);
			}
			else {
				foreach (var val in this.VALUES) {
					GUILayout.BeginHorizontal (GUILayout.MinHeight (18));
					if (val.IsToBeDeleted) {
						GUI.color = Color.red;
					}
					else
						if (val.HasChanged) {
							GUI.color = Color.yellow;
						}
					switch (val.Type) {
					case ValueType.Int:
						val.Value = EditorGUILayout.IntField (val.Name, val.IntValue, EditorStyles.textField, GUILayout.MaxWidth (500));
						break;
					case ValueType.Float:
						val.Value = EditorGUILayout.FloatField (val.Name, val.FloatValue, EditorStyles.textField, GUILayout.MaxWidth (500));
						break;
					case ValueType.String:
						val.Value = EditorGUILayout.TextField (val.Name, val.StringValue, EditorStyles.textField, GUILayout.MaxWidth (500));
						break;
					}
					GUILayout.FlexibleSpace ();
					val.Type = (ValueType)EditorGUILayout.EnumPopup (val.Type, GUILayout.MaxWidth (140));
					if (GUILayout.Button ("X", EditorStyles.miniButton, GUILayout.Width (16), GUILayout.Height (16))) {
						val.IsToBeDeleted = !val.IsToBeDeleted;
					}
					GUILayout.EndHorizontal ();
					GUI.color = Color.white;
				}
			}
			GUILayout.EndScrollView ();
		}

		/// <summary>
		/// Saves all.
		/// </summary>
		private void SaveAll () {
			if (null != this.VALUES) {

				//Loop through all the values
				foreach (var val in this.VALUES) {

					//Check if key needs to be deleted
					if (val.IsToBeDeleted) {
						LocalStore.Instance.DeleteKey (val.Name);
						continue;
					}

					//Check type
					if (null != val.Value) {
						switch (val.Type) {
							case ValueType.Int:
								LocalStore.Instance.SetInt (val.Name, val.IntValue);
								break;
							case ValueType.Float:
								LocalStore.Instance.SetFloat (val.Name, val.FloatValue);
								break;
							case ValueType.String:
								LocalStore.Instance.SetString (val.Name, val.StringValue);
								break;
							}
					}
				}

				LocalStore.Instance.Save ();
			}
		}

		/// <summary>
		/// Refreshs the data.
		/// </summary>
		private void RefreshData () {
			if (null != this.VALUES) {
				this.VALUES.Clear ();
			}
			var dictionary = LocalStore.Instance.GetSerializedData ();
			if(null != dictionary) {
				this.VALUES = LocalStorageValue.GetValuesFromDictionary(dictionary);
			}
		}

		/// <summary>
		/// Debugs the print.
		/// </summary>
		/// <param name="dictionary">Dictionary.</param>
		public void DebugPrint(List<LocalStorageValue> values) {
			if(null != values) {
				Logger.Log("[LocalStorageViewer] VALUES Count - " + values.Count);
				foreach(var val in values) {
					Logger.Log(val.ToString());
				}
			}
		}
	}
}