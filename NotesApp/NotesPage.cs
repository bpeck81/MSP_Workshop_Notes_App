using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using PCLStorage;
namespace NotesApp
{
	public class NotesPage : ContentPage
	{
		ObservableCollection<NoteFrame> notesList;
		public NotesPage (List<string> savedList)
		{
			NavigationPage.SetHasNavigationBar (this, false);
			MessagingCenter.Subscribe<CustomViewCell, string>(this,"delete", async (sender, args)=>{
				var response = await DisplayAlert("Delete", "Delete this note?","Yes","No");
				if(response){
					deleteNote(args as string);
				}
			});
			notesList = new ObservableCollection<NoteFrame> ();
			foreach (string note in savedList) {
				notesList.Add (new NoteFrame(note));
			}
			var header = new Label {
				Text = "My Notes",
				FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Start
			};
			var noteListView = new ListView{
				ItemsSource = notesList,
				ItemTemplate = new DataTemplate(typeof(CustomViewCell))
			};
			var bottomEntry = new Entry {
				Placeholder = "Enter Text Here"
			};
			bottomEntry.Completed += BottomEntry_Completed;

			Content = new StackLayout {
				Children = {
					header,
					noteListView,
					bottomEntry
				},
				Padding = new Thickness(0,20,0,0)
			};
		}

		async void BottomEntry_Completed (object sender, EventArgs e)
		{
			Entry myEntry = sender as Entry;
			notesList.Add (new NoteFrame(myEntry.Text));
			var fileSystem = FileSystem.Current.LocalStorage;
			var fileExists = await fileSystem.CheckExistsAsync ("NotesData.txt");
			if(fileExists.Equals(ExistenceCheckResult.FileExists)){
				IFile file = await fileSystem.GetFileAsync("NotesData.txt");
				var fileString = await file.ReadAllTextAsync();
				fileString +=   myEntry.Text+"\n";
				await file.WriteAllTextAsync (fileString);
			}

			myEntry.Text = "";
		}
		private async void deleteNote(string frame){
			NoteFrame removeNote = null;
			foreach (NoteFrame note in notesList) {
				if (note.noteString.Equals (frame)) {
					removeNote = note;
				}
			}
			notesList.Remove (removeNote);
			var fileSystem = FileSystem.Current.LocalStorage;
			var exists = await fileSystem.CheckExistsAsync ("NotesData.txt");
			if (exists.Equals (ExistenceCheckResult.FileExists)) {
				var file = await fileSystem.GetFileAsync ("NotesData.txt");
				var fileString = await file.ReadAllTextAsync ();
				System.Diagnostics.Debug.WriteLine (fileString);
				var fileList = new List<string>(fileString.Split ('\n'));
				string combineString = "";
				string removeString = null;
				foreach (string note in fileList) {
					if (note.Equals (frame)) {
						removeString = note;
					} else {
						if (!note.Equals ("")) {
							combineString += note + "\n";
						}
				
					}
				}
				fileList.Remove (removeString);
				await file.WriteAllTextAsync (combineString);
			}
		}
	}

	public class CustomViewCell : ViewCell{
		TapGestureRecognizer labelTGR;
		public CustomViewCell(){
			labelTGR = new TapGestureRecognizer ();

			Label label = new Label {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			label.GestureRecognizers.Add (labelTGR);
			label.SetBinding (Label.TextProperty, "noteString");
			View = new StackLayout {
				Children = { label },
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			labelTGR.Tapped += (object sender, EventArgs e) => {
				MessagingCenter.Send<CustomViewCell, string>(this,"delete",((NoteFrame)BindingContext).noteString);
			};

		
		}

	}
}


