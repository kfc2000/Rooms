using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace rooms
{
	public partial class Rooms : ContentPage
	{
		public static bool ChatRoomStarted = false;


		List<Room> roomList = new List<Room> ();

		public Rooms ()
		{
			InitializeComponent ();

			// If running in iOS, place a padding a the top of the title bar.
			//
			if (Device.OS == TargetPlatform.iOS) {
				stackTitleBar.Padding = new Thickness (20, 30, 20, 10);
			}


			// Load data from Firebase
			//
			var rooms = App.Firebase.Child ("rooms");
			rooms.On ("value", (snapshot, child, context) => {

				roomList.Clear ();
				foreach (var data in snapshot.Children) {
					if (data.Key != "DUMMYROOM") {
						roomList.Add (new Room {
							Name = data.Key,
							Desc = data.Child ("desc").Value ()
						});
					}
				}

				Device.BeginInvokeOnMainThread (() => {
					this.lvRooms.ItemSource = null;
					this.lvRooms.ItemsSource = roomList;
				});
			});


			// When the user selects the room, pop up a new modal
			// screen to show the chats happening inside.
			//
			lvRooms.ItemSelected += (sender, e) => {

				if (lvRooms.SelectedItem != null) {
					var room = this.lvRooms.SelectedItem as Room;


					ChatRoomStarted = true;
					Navigation.PushModalAsync (new ChatRoom (room));

					lvRooms.SelectedItem = null;
				}
			};
		}
	}


}

