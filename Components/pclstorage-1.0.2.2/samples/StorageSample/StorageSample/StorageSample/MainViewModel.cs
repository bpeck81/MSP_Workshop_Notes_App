using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace StorageSample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // fields
        private string fileContents;

        // constructor

        public MainViewModel()
        {
            RandomFile randomFile = new RandomFile();

            ReadContentsCommand = new Command(async () =>
            {
                // check to see if the file exists
                var exists = await randomFile.TestRandomFile();
                if (!exists)
                {
                    // create the file if it does not exist
                    await randomFile.CreateRandomFile();
                }

                // read the contents out of the file
                var value = await randomFile.LoadRandomFile();

                // update the UI
                if (!exists)
                {
                    FileContents = "Created random file: " + value;
                }
                else
                {
                    FileContents = "Existing random file: " + value;
                }
            });

            DeleteFileCommand = new Command(async () =>
            {
                // check to see if the file exists
                var exists = await randomFile.TestRandomFile();
                if (exists)
                {
                    // delete the file if it exists
                    await randomFile.DeleteRandomFile();
                }

                // update the UI
                if (!exists)
                {
                    FileContents = "No random file to delete.";
                }
                else
                {
                    FileContents = "Deleted random file.";
                }
            });
        }

        // properties

        public string FileContents
        {
            get { return fileContents; }
            set
            {
                if (fileContents != value)
                {
                    fileContents = value;
                    OnPropertyChanged("FileContents");
                }
            }
        }

        // commands

        public ICommand ReadContentsCommand { get; private set; }
        public ICommand DeleteFileCommand { get; private set; }

        // for data binding

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
