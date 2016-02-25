using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;

namespace StorageSample
{
    public class RandomFile
    {
        private Random random = new Random();

        private const string subFolderName = "MySubFolder";
        private const string fileName = "random.txt";

        public async Task CreateRandomFile()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.CreateFolderAsync(subFolderName, CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            await file.WriteAllTextAsync(random.Next().ToString());
        }

        public async Task DeleteRandomFile()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.GetFolderAsync(subFolderName);
            var file = await folder.GetFileAsync(fileName);

            await file.DeleteAsync();
        }

        public async Task<string> LoadRandomFile()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.GetFolderAsync(subFolderName);
            var file = await folder.GetFileAsync(fileName);

            return await file.ReadAllTextAsync();
        }

        public async Task<bool> TestRandomFile()
        {
            var result = false;

            var rootFolder = FileSystem.Current.LocalStorage;
            if (await rootFolder.CheckExistsAsync(subFolderName) == ExistenceCheckResult.FolderExists)
            {
                var folder = await rootFolder.GetFolderAsync(subFolderName);
                result = await folder.CheckExistsAsync(fileName) == ExistenceCheckResult.FileExists;
            }

            return result;
        }
    }
}
