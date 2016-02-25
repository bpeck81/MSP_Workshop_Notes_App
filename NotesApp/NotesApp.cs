using System;

using Xamarin.Forms;
using PCLStorage;
using System.Collections.Generic;

namespace NotesApp
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new ContentPage {
				Content = new Label {
					Text = "Loading... "
				}
			};
		
		}

		protected async override void OnStart ()
		{
			var fileSystem = FileSystem.Current.LocalStorage;				
			var fileExists = await fileSystem.CheckExistsAsync ("NotesData.txt");
			List<string> savedDataList;
			if (fileExists.Equals (ExistenceCheckResult.FileExists)) {
				IFile file = await fileSystem.GetFileAsync ("NotesData.txt");
				string fileString = await file.ReadAllTextAsync ();
				savedDataList = new List<string> (fileString.Split ('\n'));
				savedDataList.RemoveAt (savedDataList.Count - 1);

			} else {
				await fileSystem.CreateFileAsync ("NotesData.txt", CreationCollisionOption.ReplaceExisting);
				savedDataList = new List<string> ();
			}

			MainPage = new NavigationPage (new NotesPage (savedDataList));
 
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

