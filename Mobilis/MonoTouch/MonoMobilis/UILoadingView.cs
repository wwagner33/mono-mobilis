using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;

// Original idea:   http://mymonotouch.wordpress.com/2011/01/27/modal-loading-dialog/
// Based on:        http://mobiledevelopertips.com/user-interface/uialertview-without-buttons-please-wait-dialog.html

public class UILoadingView : UIAlertView
{
	private UIActivityIndicatorView activityIndicatorView;
	
	public new bool Visible {
		get;
		set;
	}
	
	public UILoadingView (string title, string message) : base (title, message, null, null, null)
	{
		this.Visible = false;
		activityIndicatorView = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);
		AddSubview (activityIndicatorView);
	}
	
	public new void Show ()
	{
		base.Show ();
		
		activityIndicatorView.Frame = new RectangleF ((Bounds.Width / 2) - 15, Bounds.Height - 60, 30, 30);
		activityIndicatorView.StartAnimating ();
		this.Visible = true;
	}
	
	public void Hide ()
	{
		this.Visible = false;
		activityIndicatorView.StopAnimating ();
		
		BeginInvokeOnMainThread (delegate () {
			DismissWithClickedButtonIndex(0, true);
		});
	}
}