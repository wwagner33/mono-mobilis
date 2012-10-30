using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Database;
using Mobilis.Lib;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib.Messages;

namespace MonoMobilis
{
	public partial class MonoMobilisViewController : UIViewController
	{

		private CoursesViewController coursesPage;
		private LoginViewModel loginViewModel;
		private UILoadingView dialog;

		public MonoMobilisViewController () : base ("MonoMobilisViewController", null)
		{

		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.NavigationController.NavigationBar.Hidden = true;
			loginViewModel = new LoginViewModel();

			this.submit.TouchUpInside += (sender, e) => 
			{
				dialog = new UILoadingView("Carregando","Por favor aguarde");
				dialog.Show();
				loginViewModel.submitLoginData(login.Text,password.Text);
			};

			ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m => 
			{
				switch (m.Content.message) 
				{
				case BaseViewMessage.MessageTypes.CONNECTION_ERROR:
					ServiceLocator.Dispatcher.invoke(() =>
					{
						if (dialog != null) 
						{
							dialog.DismissWithClickedButtonIndex(0,true);
						}
						new UIAlertView("Erro","Nao foi possivel se conectar com o servidor",null,"Fechar",null).Show();
					});
					break;
				case BaseViewMessage.MessageTypes.LOGIN_CONNECTION_OK:
					getCourses();
					break;
				case BaseViewMessage.MessageTypes.COURSE_CONNECTION_OK:
					ServiceLocator.Dispatcher.invoke(() =>
					{
						coursesPage = new CoursesViewController();
						this.NavigationController.PushViewController(this.coursesPage,true);
					});
					break;
				default:
					break;
				}            
			});
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			if (dialog != null) 
			{
				dialog.DismissWithClickedButtonIndex(0,true);
			}
		}

		public void getCourses() 
		{
			loginViewModel.requestCourses();
		}

		/* Navegacao
		 * coursesPage = new CoursesViewController();
		   this.NavigationController.PushViewController(this.coursesPage,true);
		 */


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

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			foreach (var item in this) 
			{
				var tf = item as UITextField;
				if (tf != null && tf.IsFirstResponder) 
				{
				tf.ResignFirstResponder ();
				}		
			}
		base.TouchesEnded (touches, evt);
		}
	}
}

