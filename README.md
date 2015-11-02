# LocalStorage
Simple on device persistent store.


LocalStore is a simple interface which provides basic get/set functions to persists data on the device. It also contains an EditorWindow which lets you interact with the data.

The standard interface `LocalStore.cs` can be implemented by multiple providers.

This repository currently contains 3 providers:

- `DefaultLocalStore` this provider uses the **PlayerPerfs** class provided by Unity3D to store data
- `XMLLocalStore` this provider stores data in a **XML** file.
- `JSONLocalStore` this provider stores data in a **JSON** file.
