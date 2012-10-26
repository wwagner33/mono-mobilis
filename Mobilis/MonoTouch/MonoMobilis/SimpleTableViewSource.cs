using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace MonoMobilis
{
	public class SimpleTableViewSource<T> : UITableViewSource
	{
		private List<T> rows;
		
		public SimpleTableViewSource (List<T> list) 
		{ 
			rows = list; 
		}
		
		public override int RowsInSection (UITableView tableview, int section) 
		{ 
			return rows.Count; 
		}
		
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{  
			UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default,"mycell"); 
			cell.TextLabel.Text = rows[indexPath.Row].ToString();
			return cell;
		} 
	}
}

