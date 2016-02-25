# File System Plugin for Xamarin and Windows details

The File System Plugin for Xamarin and Windows provides a consistent, portable set 
of local file IO APIs for .NET, Windows Phone, Windows Store, Xamarin.iOS, 
Xamarin.Android, and Silverlight. This makes it easier to create cross-platform 
.NET libraries and apps.

Different .NET platforms have different APIs for accessing the file system or
an app-local persisted storage area. The full .NET Framework provides the
standard file and directory APIs (in the System.IO namespace), Silverlight and
Windows Phone provide isolated storage APIs, and WinRT provides storage APIs in
the Windows.Storage namespace.

Here is a sample showing how you can use the File System Plugin for Xamarin and Windows
to create a folder and write to a text file in that folder:

```
public async Task CreateRealFileAsync()
{
    // get hold of the file system
    IFolder rootFolder = FileSystem.Current.LocalStorage;
    
    // create a folder, if one does not exist already
    IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);
    
    // create a file, overwriting any existing file
    IFile file = await folder.CreateFileAsync("MyFile.txt", CreationCollisionOption.ReplaceExisting);
    
    // populate the file with some text
    await file.WriteAllTextAsync("Sample Text...");
}
```

## APIs

The primary APIs in the File System Plugin for Xamarin and Windows are the 
`IFile`, `IFolder`, and `IFileSystem` interfaces. The APIs should be
mostly self-explanatory and should feel very familiar if you have used 
the WinRT storage APIs.

The `IFileSystem` interface is the main API entry point. You can get an instance
of the implementation for the current platform with the `FileSystem.Current`
property.
