using System;

using Xamarin.Forms;
using FirebaseSharp.Portable;



namespace rooms
{
	public class App : Application
	{
		public static FirebaseApp Firebase = new FirebaseApp (new Uri ("https://nyprooms.firebaseio.com/"));
		public App ()
		{
			// The root page of your application
			MainPage = new Rooms ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
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

