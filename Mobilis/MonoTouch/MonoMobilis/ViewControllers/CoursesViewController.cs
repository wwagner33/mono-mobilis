using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib.Messages;
using Mobilis.Lib;

namespace MonoMobilis
{
	public partial class CoursesViewController : UIViewController
	{
		private UITableView table;
		protected static CoursesViewModel coursesViewModel;
		public static UINavigationController navController;

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
			navController = this.NavigationController;
			this.NavigationController.NavigationBar.Hidden = true;
			coursesViewModel = new CoursesViewModel();
			table = new UITableView(View.Bounds);
			table.AutoresizingMask = UIViewAutoresizing.All;
			table.Source = new TableViewSource2(coursesViewModel.listContent);
			Add(table);

			ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m =>
			{
				switch (m.Content.message)
				{
				case BaseViewMessage.MessageTypes.CONNECTION_ERROR:
					ServiceLocator.Dispatcher.invoke(() =>
					                                 {
						//Toast.MakeText(this, "Erro de conexão", ToastLength.Short).Show();
						//dialog.Dismiss();
					});
					break;
				case BaseViewMessage.MessageTypes.CLASS_CONNECTION_OK:
					ServiceLocator.Dispatcher.invoke(() =>
					{
						//intent = new Intent(this, typeof(ClassActivity));
						//StartActivity(intent);
						new UIAlertView("Row Selected","Para a lista de turmas", null, "OK", null).Show();
					});
					break;
				case BaseViewMessage.MessageTypes.COURSE_CONNECTION_OK:
					ServiceLocator.Dispatcher.invoke(() =>
					{
						//TODO refresh.
					});
					break;
				default:
					break;
				}
			});


		}

		/*
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			new UIAlertView("Row Selected", tableItems[indePath.Row], null, "OK", null).Show();
			tableView.DeselectRow (indexPath, true); // normal iOS behaviour is to remove the blue highlight
		}
		*/

		public class TableViewSource2 : UITableViewSource
		{
			private List<Course> teste;

			public TableViewSource2(List<Course> courses)
			{
				teste = courses;
			}
			
			public override int RowsInSection (UITableView tableview, int section) 
			{ 
				return teste.Count;
			}
			
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{  
				UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default,"mycell"); 
				cell.TextLabel.Text = teste[indexPath.Row].ToString();
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				ClassesViewController classViewController = new ClassesViewController();

				if (coursesViewModel.existClasses(indexPath.Row)) 
				{
					navController.PushViewController(classViewController,true);
				} 
				else 
				{
					new UILoadingView("Carregando","Por favor aguarde").Show();
					coursesViewModel.requestClass();
					//new UIAlertView("Row Selected","Nao existem cursos", null, "OK", null).Show();
				}
			}
		}
	}
}

