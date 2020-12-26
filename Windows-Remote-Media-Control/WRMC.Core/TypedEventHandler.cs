using System;

namespace WRMC.Core {
	/// <summary>
	/// Template event handler to define a strongly typed event handler
	/// </summary>
	/// <typeparam name="TSender">The type of the sender object.</typeparam>
	/// <typeparam name="TEventArgs">The type of the event args object. As to derive <see cref="EventArgs"/></typeparam>
	/// <param name="sender">The sender of the event.</param>
	/// <param name="e">The event args of the event</param>
	public delegate void TypedEventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;
}