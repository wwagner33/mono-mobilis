
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using BasicTable;

namespace MonoMobilis
{
	public partial class CoursesViewController : UIViewController
	{
		UITableView table;
		List<Course> courses;
		CourseDao courseDao;


		public CoursesViewController () : base ("CoursesViewController", null)
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
			courseDao = new CourseDao();
			this.courses = courseDao.getAllCourses();
			System.Diagnostics.Debug.WriteLine("Número de cursos = " + courses.Count);
			table = new UITableView(View.Bounds);
			table.AutoresizingMask = UIViewAutoresizing.All;
			createTableItems();
			Add(table);

		}

		public void createTableItems ()
		{
			List<string> tableItems = new List<string> ();

			foreach (var course in courses) {
				tableItems.Add(course.name);
			}
			table.Source = new TableSource(tableItems.ToArray());
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

