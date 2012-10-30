using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Mobilis.Lib.Database;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;
using Mobilis.Lib.Messages;

namespace MonoMobilis
{
	public partial class ClassesViewController : UIViewController
	{
		private UITableView table;
		private static ClassViewModel classViewModel;
		private static UINavigationController navController;
		private static DiscussionViewController discussionViewController;
		private static UILoadingView dialog;

		public ClassesViewController () : base ("ClassesViewController", null)
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
			navController = this.NavigationController;
			classViewModel = new ClassViewModel();
			table = new UITableView(new RectangleF (
				0,50, this.View.Frame.Width,
				this.View.Frame.Height));
			table.AutoresizingMask = UIViewAutoresizing.All;
			table.Source = new TableViewSource2(classViewModel.classes);
			Add(table);

			ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m =>
			{
				switch (m.Content.message)
				{
				case BaseViewMessage.MessageTypes.CONNECTION_ERROR:
					ServiceLocator.Dispatcher.invoke(() =>
					{
					});
					break;
					
				case BaseViewMessage.MessageTypes.DISCUSSION_CONNECTION_OK:
					ServiceLocator.Dispatcher.invoke(() =>
					{
						NavigationController.PushViewController(discussionViewController,true);
					});
					break;
				default:
					break;
				}
			});
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

		private class TableViewSource2 : UITableViewSource
		{
			private List<Class> content;

			
			public TableViewSource2(List<Class> classes)
			{
				content = classes;
			}
			
			public override int RowsInSection (UITableView tableview, int section) 
			{ 
				return content.Count;
			}
			
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{  
				UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default,"mycell"); 
				cell.TextLabel.Text = content[indexPath.Row].ToString();
				return cell;
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				tableView.DeselectRow(indexPath, true);
				discussionViewController = new DiscussionViewController();

				if (classViewModel.existDiscussionsAtClass(indexPath.Row)) 
				{
					navController.PushViewController(discussionViewController,true);
				} 
				else 
				{
					dialog = new UILoadingView("Carregando","Por favor aguarde");
					dialog.Show();
					classViewModel.requestDiscussions();
				}
			}
		}

	}
}

