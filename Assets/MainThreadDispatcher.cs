
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
	private static MainThreadDispatcher _instance = null;

	private static Queue<Action> _actionQueue;

	private void Awake()
	{
		_actionQueue = new();

		if ( _instance == null )
		{
			_instance = this;

			DontDestroyOnLoad( gameObject );
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	public void Update()
	{
		lock ( _actionQueue )
		{
			while ( _actionQueue.Count > 0 )
			{
				_actionQueue.Dequeue().Invoke();
			}
		}
	}

	public void BeginInvoke( IEnumerator action )
	{
		lock ( _actionQueue )
		{
			_actionQueue.Enqueue( () =>
			{
				StartCoroutine( action );
			} );
		}
	}

	public void BeginInvoke( Action action )
	{
		BeginInvoke( ActionCoroutine( action ) );
	}

	public Task BeginInvokeAsync( Action action )
	{
		var boolTaskCompletionSource = new TaskCompletionSource<bool>();

		void WrappedAction()
		{
			try
			{
				action();

				boolTaskCompletionSource.TrySetResult( true );
			}
			catch ( Exception exception )
			{
				boolTaskCompletionSource.TrySetException( exception );
			}
		}

		BeginInvoke( ActionCoroutine( WrappedAction ) );

		return boolTaskCompletionSource.Task;
	}

	IEnumerator ActionCoroutine( Action action )
	{
		action();

		yield return null;
	}

	public static bool Exists()
	{
		return _instance != null;
	}

	public static MainThreadDispatcher Instance()
	{
		return _instance;
	}
}
