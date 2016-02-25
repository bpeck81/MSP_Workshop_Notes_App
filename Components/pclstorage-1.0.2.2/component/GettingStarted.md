# Getting Started with File System Plugin for Xamarin and Windows

The File System Plugin for Xamarin and Windows provides a consistent, portable set 
of local file IO APIs for .NET, Windows Phone, Windows Store, Xamarin.iOS,
Xamarin.Android, and Silverlight. This makes it easier to create cross-platform 
.NET libraries and apps.

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

## Background information

Different .NET platforms have different APIs for accessing the file system or
an app-local persisted storage area. The full .NET Framework provides the
standard file and directory APIs (in the System.IO namespace), Silverlight and
Windows Phone provide isolated storage APIs, and WinRT provides storage APIs in
the Windows.Storage namespace.

These differing APIs make it harder to write cross-platform code. Traditionally,
you could handle this via conditional compilation. However, that means you can't
take advantage of Portable Class Libraries, and in any case may not scale well
as your code gets complex (and especially because for WinRT you need to use
async APIs).

Alternatively, you can create an abstraction for the functionality you need
across platforms, and implement the abstraction for each platform you need to
use. This approach allows you to use Portable Class Libraries, and in general
makes your code cleaner and more maintainable by isolating the platform-specific
pieces instead of having them sprinkled arbitrarily throughout your code.

Writing an abstraction layer is a bit of a barrier to entry to writing
cross-platform code, and there's no reason everyone should have to do it
separately for functionality as commonly needed as local file IO. The
File System Plugin for Xamarin and Windows aims to provide a common abstraction
that is easy to take advantage of.

## APIs

The primary APIs in the File System Plugin for Xamarin and Windows are the 
`IFile`, `IFolder`, and `IFileSystem` interfaces. The APIs should be mostly
self-explanatory and should feel very familiar if you have used the WinRT 
storage APIs.

The `IFileSystem` interface is the main API entry point. You can get an instance
of the implementation for the current platform with the `FileSystem.Current`
property.

Here are some of the basic storage types:

```
namespace PCLStorage
{
    public static class FileSystem
    {
        public static IFileSystem Current { get; }
    }

    public interface IFileSystem
    {
        IFolder LocalStorage { get; }
        IFolder RoamingStorage { get; }

        Task<IFile> GetFileFromPathAsync(string path);
        Task<IFolder> GetFolderFromPathAsync(string path);
    }

    public interface IFolder
    {
        string Name { get; }
        string Path { get; }

        Task<IFile> CreateFileAsync(string desiredName, CreationCollisionOption option);
        Task<IFile> GetFileAsync(string name);
        Task<IList<IFile>> GetFilesAsync();

        Task<IFolder> CreateFolderAsync(string desiredName, CreationCollisionOption option);
        Task<IFolder> GetFolderAsync(string name);
        Task<IList<IFolder>> GetFoldersAsync();

        Task<ExistenceCheckResult> CheckExistsAsync(string name, CancellationToken cancellationToken);

        Task DeleteAsync();
    }

    public interface IFile
    {
        string Name { get; }
        string Path { get; }

        Task<Stream> OpenAsync(FileAccess fileAccess);
        Task DeleteAsync();
        Task RenameAsync(string newName, NameCollisionOption collisionOption, CancellationToken cancellationToken);
        Task MoveAsync(string newPath, NameCollisionOption collisionOption, CancellationToken cancellationToken);
    }
} 

```