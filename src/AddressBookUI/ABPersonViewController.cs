// 
// ABPersonViewController.cs: 
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
//

#nullable enable

using System;

using AddressBook;
using Foundation;
using ObjCRuntime;

namespace AddressBookUI {
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ABPersonViewPerformDefaultActionEventArgs : EventArgs {
		public ABPersonViewPerformDefaultActionEventArgs (ABPerson person, ABPersonProperty property, int? identifier)
		{
			Person = person;
			Property = property;
			Identifier = identifier;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABPerson Person { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABPersonProperty Property { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int? Identifier { get; private set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool ShouldPerformDefaultAction { get; set; }
	}

	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	class InternalABPersonViewControllerDelegate : ABPersonViewControllerDelegate {

		internal EventHandler<ABPersonViewPerformDefaultActionEventArgs>? performDefaultAction;

		public InternalABPersonViewControllerDelegate ()
		{
			IsDirectBinding = false;
		}

		[Preserve (Conditional = true)]
		public override bool ShouldPerformDefaultActionForPerson (ABPersonViewController personViewController, ABPerson person, int propertyId, int identifier)
		{
			ABPersonProperty property = ABPersonPropertyId.ToPersonProperty (propertyId);
			int? id = identifier == ABRecord.InvalidPropertyId ? null : (int?) identifier;

			var e = new ABPersonViewPerformDefaultActionEventArgs (person, property, id);
			personViewController.OnPerformDefaultAction (e);
			return e.ShouldPerformDefaultAction;
		}
	}

	partial class ABPersonViewController {

		ABPerson? displayedPerson;
		/// <summary>Returns the <see cref="T:AddressBook.ABPerson" /> associated with the displayed data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABPerson? DisplayedPerson {
			get {
				MarkDirty ();
				return BackingField.Get (ref displayedPerson, _DisplayedPerson, h => new ABPerson (h, AddressBook));
			}
			set {
				_DisplayedPerson = BackingField.Save (ref displayedPerson, value);
				MarkDirty ();
			}
		}

		DisplayedPropertiesCollection? displayedProperties;
		/// <summary>Gets the collection of properties that are displayed about the <see cref="P:AddressBookUI.ABPersonViewController.DisplayedPerson" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public DisplayedPropertiesCollection? DisplayedProperties {
			get {
				if (displayedProperties is null) {
					displayedProperties = new DisplayedPropertiesCollection (
							() => _DisplayedProperties,
							v => _DisplayedProperties = v);
					MarkDirty ();
				}
				return displayedProperties;
			}
		}

		ABAddressBook? addressBook;
		/// <summary>Gets or sets the <see cref="T:AddressBook.ABAddressBook" /> that is the store for the data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABAddressBook? AddressBook {
			get {
				MarkDirty ();
				return BackingField.Get (ref addressBook, _AddressBook, h => new ABAddressBook (h, false));
			}
			set {
				_AddressBook = BackingField.Save (ref addressBook, value);
				MarkDirty ();
			}
		}

		public void SetHighlightedItemForProperty (ABPersonProperty property, int? identifier)
		{
			SetHighlightedItemForProperty (
					ABPersonPropertyId.ToId (property),
					identifier ?? ABRecord.InvalidPropertyId);
		}

		public void SetHighlightedProperty (ABPersonProperty property)
		{
			SetHighlightedItemForProperty (
					ABPersonPropertyId.ToId (property),
					ABRecord.InvalidPropertyId);
		}

		InternalABPersonViewControllerDelegate EnsureEventDelegate ()
		{
			var d = WeakDelegate as InternalABPersonViewControllerDelegate;
			if (d is null) {
				d = new InternalABPersonViewControllerDelegate ();
				WeakDelegate = d;
			}
			return d;
		}

		protected internal virtual void OnPerformDefaultAction (ABPersonViewPerformDefaultActionEventArgs e)
		{
			var h = EnsureEventDelegate ().performDefaultAction;
			if (h is not null)
				h (this, e);
		}

		public event EventHandler<ABPersonViewPerformDefaultActionEventArgs> PerformDefaultAction {
			add { EnsureEventDelegate ().performDefaultAction += value; }
			remove { EnsureEventDelegate ().performDefaultAction -= value; }
		}
	}
}
