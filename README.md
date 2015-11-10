# LocalStorage
Simple on device persistent store.


LocalStore provides basic get/set functions to persists data on the device. It also contains an EditorWindow which lets you interact with the data.

The standard interface `ILocalStore.cs` can be implemented by multiple providers.

This repository currently contains 3 providers:

- `DefaultLocalStore` this provider uses the **PlayerPerfs** class provided by Unity3D to store data
- `XMLLocalStore` this provider stores data in a **XML** file.
- `JSONLocalStore` this provider stores data in a **JSON** file.

## Download
You can download the [Unity Plugin](https://github.com/JuneSoftware/LocalStorage/raw/master/June.LocalStorage.unitypackage) and import it directly in your project.

Alternatively, you could also clone this repository.

## Usage
The [`June/LocalStorage/LocalStore.cs`](https://github.com/JuneSoftware/LocalStorage/blob/master/Assets/June/LocalStorage/LocalStore.cs) file contains a static `Instance` property which is used to initialise the type of provider. In the snippet of code below we are initialising the [`DefaultLocalStore`](https://github.com/JuneSoftware/LocalStorage/blob/master/Assets/June/LocalStorage/Providers/DefaultLocalStore.cs) provider.

```csharp
/// <summary>
/// Gets the local storage instance.
/// </summary>
public static ILocalStore Instance {
	get {
		if (null == _Instance) {
			_Instance = InitializeProvider();
		}
		return _Instance;
	}
}

/// <summary>
/// This method initializes the provider.
/// </summary>
/// <returns>The provider.</returns>
public static ILocalStore InitializeProvider() {
	//This provider uses the PlayerPerfs present in Unity3d as the persistent store
	return new LocalStorage.Providers.DefaultLocalStore();
}
```

### Saving data
The basic methods to save data are:

```csharp
June.LocalStore.Instance.SetInt("my_int_key", 123);
June.LocalStore.Instance.SetFloat("my_float_key", 123.321f);
June.LocalStore.Instance.SetString("my_string_key", "some string");
```

### Retrieving Data
The basic methods to retrieve data are:

```csharp
int intVal = June.LocalStore.Instance.GetInt("my_int_key");
float floatVal = June.LocalStore.Instance.GetFloat("my_float_key");
string strVal = June.LocalStore.Instance.GetString("my_string_key");
```

### Helper Methods
Some helper methods provided:

```csharp
bool HasKey (string key) // Checks if the key is present in the store

void DeleteKey (string key) // Deletes a key from the store
void DeleteAll () // Deletes all the keys in the store

void Increment (string key) // Increments the int value by 1
void Decrement (string key) // Decrements the int value by 1

string[] GetStringArray (string key) // Fetches a string array
void SetStringArray (string key, string[] values) // Persists a string array to a key

bool GetBool (string key) // Fetches a boolean value
void SetBool (string key, bool value) // Stores a boolean value

// Checks if the key exists, if not returns the default value for the type.
int    GetIntOrDefault    (string key) 
float  GetFloatOrDefault  (string key)
string GetStringOrDefault (string key)
bool   GetBoolOrDefault   (string key)
```


## Screenshots

Unity Editor showing Demo scene and Local Storage Editor Window docked on the right.

![Image of Unity Editor](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/UnityEditor.png)

The LocalStorage Editor Window can be opened from:
`June -> Local Storage Viewer`

![Image of Local Storage Editor Window](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/1EditorWindow.png)

Adding an item to the local store.

![Image of Local Storage Editor Window](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/2EditorWindowCreate.png)

New/Modified keys are highlighted in yellow.

![Image of Local Storage Editor Window](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/3EditorWindowCreated.png)

The New/Modified key-values are in memory and will get written to the store only when `Save All` is clicked. When all the keys are saved they revert back to their default colour.

![Image of Local Storage Editor Window](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/4EditorWindowSaved.png)

A key can be deleted by clicking the `x` button. Once a key has been flagged for deletion it will be highlighted in red.

![Image of Local Storage Editor Window](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/5EditorWindowDeleting.png)

When the `Save All` button is clicked the keys marked for deletion will be removed.

![Image of Local Storage Editor Window](https://raw.githubusercontent.com/JuneSoftware/LocalStorage/master/screenshots/6EditorWindowDeleted.png)


## Writing your custom provider

To create a custom local storage provider you need to inherit the [`June/LocalStorage/ILocalStore.cs`](https://github.com/JuneSoftware/LocalStorage/blob/master/Assets/June/LocalStorage/ILocalStore.cs) class and implement its abstract methods.

You can also use one of the providers already present as a base and override certain methods.

In the following example we use the [`JSONLocalStore.cs`](https://github.com/JuneSoftware/LocalStorage/blob/master/Assets/June/LocalStorage/Providers/JSONLocalStore.cs) as our base and override the `Serialize` and `Deserealize` methods to encrypt and decrypt our data.

```csharp
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

```