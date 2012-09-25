using System;
using Mobilis.Lib;
using MonoTouch.Foundation;

namespace MonoMobilis
{
	public class DispatchAdapter : IDispatchOnUIThread
	{
		private readonly NSObject _owner;

		public DispatchAdapter(NSObject owner)
		{
			_owner = owner;
		}

		public void invoke (Action action)
		{
			_owner.BeginInvokeOnMainThread(new NSAction(action));
		}
	}
}

