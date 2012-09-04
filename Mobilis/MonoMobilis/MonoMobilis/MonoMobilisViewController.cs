using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoMobilis
{
	public partial class MonoMobilisViewController : UIViewController
	{
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
			this.submit.TouchUpInside += (sender, e) => {
				System.Diagnostics.Debug.WriteLine("Teste");
				System.Diagnostics.Debug.WriteLine("Login = " + login.Text);
				System.Diagnostics.Debug.WriteLine("Password = " + password.Text);
			};
		}

		partial void actionSubmit (NSObject sender)
		{
			System.Diagnostics.Debug.WriteLine("Teste2");
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

