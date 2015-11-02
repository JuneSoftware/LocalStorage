using UnityEngine;
using System.Collections;

/// <summary>
/// Demo scene script.
/// </summary>
using June;


public class DemoLocalStorage : MonoBehaviour {

	const float LABEL_WIDTH = 50f;
	const float TEXTBOX_WIDTH = 100f;
	const float BUTTON_WIDTH = 100f;

	string _key = string.Empty;
	string _value = string.Empty;

	System.Text.StringBuilder _log;

	void Start () {
		_key = string.Empty;
		_value = string.Empty;
		_log = new System.Text.StringBuilder();
		Log ("Ready, Using: " + LocalStore.Instance.ToString());
	}

	void OnGUI() {
		RenderDemoScene();
	}

	/// <summary>
	/// Renders the demo scene.
	/// </summary>
	void RenderDemoScene() {
		GUILayout.BeginVertical();
		RenderTitle();
		RenderUserInputs();
		RenderLog();
		GUILayout.EndVertical();
	}

	/// <summary>
	/// Renders the title.
	/// </summary>
	void RenderTitle() {
		GUILayout.BeginHorizontal(); 
		GUILayout.Label("LocalStorage Demo");
		GUILayout.EndHorizontal();
	}
	
	/// <summary>
	/// Renders the user inputs.
	/// </summary>
	void RenderUserInputs() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Key", GUILayout.Width(LABEL_WIDTH));
		_key = GUILayout.TextField(_key, GUILayout.Width(TEXTBOX_WIDTH));
		RenderGetButtons();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Value", GUILayout.Width(LABEL_WIDTH));
		_value = GUILayout.TextField(_value, GUILayout.Width(TEXTBOX_WIDTH));
		RenderSetButtons();
		GUILayout.EndHorizontal();

		if(GUILayout.Button("GetSerializedData", GUILayout.Width(BUTTON_WIDTH * 3))) {
			Log ("Serialized Data: \n" + LocalStore.Instance.GetSerializedDataJSON());
		}
	}

	Vector2 _LogScroll;
	/// <summary>
	/// Renders the log.
	/// </summary>
	void RenderLog() {
		GUILayout.Label("--- --- --- LOG --- --- ---", GUILayout.ExpandWidth(true));
		_LogScroll = GUILayout.BeginScrollView(_LogScroll, false, true);
		GUILayout.Label(_log.ToString(), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
	}

	/// <summary>
	/// Renders the get buttons.
	/// </summary>
	void RenderGetButtons() {
		if(GUILayout.Button("GetInt", GUILayout.Width(BUTTON_WIDTH))) {
			if(LocalStore.Instance.HasKey(_key)) {
				_value = LocalStore.Instance.GetInt(_key).ToString();
				Log ("GetInt key `" + _key + "` value `" + _value + "`");
			}
			else {
				Log ("Key `" + _key + "` not found");
			}
		}

		if(GUILayout.Button("GetFloat", GUILayout.Width(BUTTON_WIDTH))) {
			if(LocalStore.Instance.HasKey(_key)) {
				_value = LocalStore.Instance.GetFloat(_key).ToString();
				Log ("GetFloat key `" + _key + "` value `" + _value + "`");
			}
			else {
				Log ("Key `" + _key + "` not found");
			}
		}

		if(GUILayout.Button("GetString", GUILayout.Width(BUTTON_WIDTH))) {
			if(LocalStore.Instance.HasKey(_key)) {
				_value = LocalStore.Instance.GetString(_key);
				Log ("GetString key `" + _key + "` value `" + _value + "`");
			}
			else {
				Log ("Key `" + _key + "` not found");
			}
		}

		if(GUILayout.Button("DeleteKey", GUILayout.Width(BUTTON_WIDTH))) {
			if(LocalStore.Instance.HasKey(_key)) {
				LocalStore.Instance.DeleteKey(_key);
				Log ("DeleteKey key `" + _key + "` success");
			}
			else {
				Log ("Key `" + _key + "` not found");
			}
		}
	}

	/// <summary>
	/// Renders the set buttons.
	/// </summary>
	void RenderSetButtons() {
		if(GUILayout.Button("SetInt", GUILayout.Width(BUTTON_WIDTH))) {
			LocalStore.Instance.SetInt(_key, int.Parse(_value));
			Log ("SetInt key `" + _key + "` value `" + _value + "`");
		}
		
		if(GUILayout.Button("SetFloat", GUILayout.Width(BUTTON_WIDTH))) {
			LocalStore.Instance.SetFloat(_key, float.Parse(_value));
			Log ("SetFloat key `" + _key + "` value `" + _value + "`");
		}
		
		if(GUILayout.Button("SetString", GUILayout.Width(BUTTON_WIDTH))) {
			LocalStore.Instance.SetString(_key, _value);
			Log ("SetString key `" + _key + "` value `" + _value + "`");
		}

		if(GUILayout.Button("DeleteAll", GUILayout.Width(BUTTON_WIDTH))) {
			LocalStore.Instance.DeleteAll();
			Log ("DeleteAll success");
		}
	}

	/// <summary>
	/// Log the specified message.
	/// </summary>
	/// <param name="message">Message.</param>
	void Log(string message) {
		_log.AppendFormat("[{0:HH:mm:ss}] {1}", System.DateTime.Now, message);
		_log.AppendLine();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
