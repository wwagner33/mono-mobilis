using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.ViewModel;

namespace MonoMobilis
{
	public partial class CoursesViewController : UIViewController
	{
		private UITableView table;
		private CoursesViewModel coursesViewModel;

		public CoursesViewController () : base ("CoursesViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();			
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
			NavigationController.ToolbarHidden = true;
			coursesViewModel = new CoursesViewModel();
			table = new UITableView(View.Bounds);
			table.AutoresizingMask = UIViewAutoresizing.All;
			table.Source = new SimpleTableViewSource<Course>(coursesViewModel.listContent);
			Add(table);
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

