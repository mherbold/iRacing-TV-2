using System;
using System.Collections;
using System.Linq;

namespace IRSDKSharper
{
	public class EventTracksAsList : IList
	{
		private readonly EventSystem eventSystem;

		public EventTracksAsList( EventSystem eventSystem )
		{
			this.eventSystem = eventSystem;
		}

		public object this[ int index ]
		{
			get
			{
				return eventSystem.Tracks.ElementAt( index ).Key;
			}

			set => throw new NotImplementedException();
		}

		public bool IsFixedSize => true;

		public bool IsReadOnly => true;

		public int Count
		{
			get
			{
				return eventSystem.Tracks.Count;
			}

			set => throw new NotImplementedException();
		}

		public bool IsSynchronized => false;

		public object SyncRoot => throw new NotImplementedException();

		public int Add( object value )
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains( object value )
		{
			throw new NotImplementedException();
		}

		public void CopyTo( Array array, int index )
		{
			throw new NotImplementedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public int IndexOf( object value )
		{
			throw new NotImplementedException();
		}

		public void Insert( int index, object value )
		{
			throw new NotImplementedException();
		}

		public void Remove( object value )
		{
			throw new NotImplementedException();
		}

		public void RemoveAt( int index )
		{
			throw new NotImplementedException();
		}
	}
}
