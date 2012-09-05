using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Database;
using Mobilis.Lib;

namespace MonoMobilis
{
	public partial class MonoMobilisViewController : UIViewController
	{
		private UITextField teste;
		private LoginService loginService;
		private CourseService courseService;
		private CourseDao courseDao;
		private CoursesViewController coursesPage;


		public MonoMobilisViewController () : base ("MonoMobilisViewController", null)
		{

		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			loginService = new LoginService();
			courseService = new CourseService();
			courseDao = new CourseDao();
			/*
			teste = new UITextField();
			teste.ShouldReturn = delegate
			{
			teste.ResignFirstResponder();
			return true;
			};
			View.AddSubview(teste);
			*/

			this.submit.TouchUpInside += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("Teste");
				System.Diagnostics.Debug.WriteLine("Login = " + login.Text);
				System.Diagnostics.Debug.WriteLine("Password = " + password.Text);
				string loginData = JSON.generateLoginObject(login.Text,password.Text);
				System.Diagnostics.Debug.WriteLine("Login data = " + loginData);
				loginService.getToken(login.Text,password.Text,r => {
					var enumerator = r.Value.GetEnumerator();
                    enumerator.MoveNext();
                    string token = enumerator.Current;
                    System.Diagnostics.Debug.WriteLine("Token = " + enumerator.Current);
					getCourses(token);
				});
			};
		}

		public void getCourses(string token) 
		{
			courseService.getCourses("curriculum_units/list.json",token,r => {
				courseDao.insertAll(r.Value);
                System.Diagnostics.Debug.WriteLine("Insert OK");
				ServiceLocator.Dispatcher.invoke(() => {
					if (this.coursesPage ==	null) 
				{
					coursesPage = new CoursesViewController();	
				}
				this.NavigationController.PushViewController(this.coursesPage,true);
				});
			});
		}


		partial void actionSubmit (NSObject sender)
		{
			System.Diagnostics.Debug.WriteLine("Teste");
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

