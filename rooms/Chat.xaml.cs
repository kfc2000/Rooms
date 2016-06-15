using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Xamarin.Forms;

namespace rooms
{
	public partial class ChatRoom : ContentPage
	{
		Room room = null;

		ObservableCollection<ChatMessage> chatMessageList = new ObservableCollection<ChatMessage> ();

		/// <summary>
		/// Adds or update messages.
		/// </summary>
		/// <returns></returns>
		private void AddOrUpdateMessages ()
		{
			var messagesRef = App.Firebase.Child ("messages/" + room.Name + "/");
			messagesRef.OrderByKey ().LimitToLast (100).On ("child_added", (snapshot, child, context) => {

				System.Diagnostics.Debug.WriteLine ("snapshot.key = " + snapshot.Key);
				System.Diagnostics.Debug.WriteLine ("snapshot.msg = " + snapshot.Child("msg").Value());
				System.Diagnostics.Debug.WriteLine ("snapshot.name = " + snapshot.Child ("name").Value ());

				chatMessageList.Add (new ChatMessage {
					ID = snapshot.Key,
					Name = snapshot.Child ("name").Value (),
					Message = snapshot.Child ("msg").Value ()
				});


				Device.BeginInvokeOnMainThread (() => {
					lvChatMessages.ItemsSource = chatMessageList;
					lvChatMessages.ScrollTo (chatMessageList [chatMessageList.Count - 1], ScrollToPosition.End, false);
				});
			});
		}


		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <returns>The message.</returns>
		private void SendMessage (string name, string msg)
		{
			if (name.Trim () == "" || msg.Trim () == "")
				return;
			
			Random r = new Random ();
			DateTime now = DateTime.Now;
			string key = now.ToString ("yyyyMMdd-HHmmss-") + (r.Next () % 10000000).ToString("0000000");
			var messagesRef = App.Firebase.Child ("messages/" + room.Name + "/" + key + "/");

			string data = String.Format("{{ \"name\" : \"{0}\", \"msg\" : \"{1}\" }}", name.Replace ('\"', '\''), msg.Replace('\"', '\''));
			messagesRef.Set (data);
		}


		public ChatRoom (Room room)
		{
			InitializeComponent ();

			// If running in iOS, place a padding a the top of the title bar.
			//
			if (Device.OS == TargetPlatform.iOS) {
				stackTitleBar.Padding = new Thickness (20, 30, 20, 10);
			}

			// Shows the room name
			//
			this.room = room;
			lblRoomName.Text = room.Name;

			// When the user taps the Close button.
			//
			btnClose.Clicked += (sender, e) => {
				Rooms.ChatRoomStarted = false;
				Navigation.PopModalAsync ();
			};

			// Load all the chat messages, and auto update
			//
			AddOrUpdateMessages ();

			// When the send button is clicked, add the message
			// to Firebase
			//
			btnSend.Clicked += (sender, e) => {
				if (txtNickname.Text == null || txtMessage.Text == null ||
					txtNickname.Text.Trim () == "" || txtMessage.Text.Trim () == "") {
					DisplayAlert ("Oops!", "Please enter name and message!", "OK!");
					return;
				}
				SendMessage (txtNickname.Text, txtMessage.Text);
			};

			// When on iOS, if any of the text boxes are in focus,
			// set the padding of the bottom stack view to avoid
			// blocking the keyboard.
			// (Android doesn't seem to have this problem as the
			// view will auto size itself!)
			//
			if (Device.OS == TargetPlatform.iOS) {
				txtMessage.Focused += (sender, e) => {
					stackMessage.HeightRequest = 380;
					lvChatMessages.ScrollTo (chatMessageList [chatMessageList.Count - 1], ScrollToPosition.End, true);
				};

				txtNickname.Focused += (sender, e) => {
					stackMessage.HeightRequest = 380;
					lvChatMessages.ScrollTo (chatMessageList [chatMessageList.Count - 1], ScrollToPosition.End, true);
				};

				txtMessage.Unfocused += (sender, e) => {
					stackMessage.HeightRequest = 120;
				};

				txtNickname.Unfocused += (sender, e) => {
					stackMessage.HeightRequest = 120;
				};
			}
		}
	}
}

