// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// Trigger handler.
	/// </summary>
	public delegate void TriggerHandler(ITrigger trigger, ITransient reason);

	/// <summary>
	/// A Trigger Completes itself when any of the objects in it are Completed
	/// </summary>
	public interface ITrigger : IGroup
	{
		/// <summary>
		/// Occurs when any of the objects added to the trigger are deleted.
		/// </summary>
		event TriggerHandler Tripped;
	}
}