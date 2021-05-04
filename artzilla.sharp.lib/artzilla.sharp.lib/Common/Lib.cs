using System;

namespace ArtZilla.Net.Core {
	/// <summary> </summary>
	public struct ActionResult {
		/// <summary> </summary>
		public Exception Exception { get; }
		
		/// <summary> </summary>
		public bool IsOk => Exception == null;
		
		/// <summary> </summary>
		public ActionResult(Exception exception = null) => Exception = exception;
	}

	
	/// <summary> </summary>
	public struct FuncResult<T> {
		/// <summary> </summary>
		public Exception Exception { get; }
		/// <summary> </summary>
		public bool IsOk => Exception == null;
		/// <summary> </summary>
		public T Result { get; }
		
		/// <summary> </summary>
		public FuncResult(T result) {
			Result = result;
			Exception = null;
		}
		
		/// <summary> </summary>
		public FuncResult(Exception exception = null) {
			Result = default(T);
			Exception = exception;
		}
	}
	
	/// <summary> </summary>
	public static class Lib {
		/// <summary> </summary>
		public static ActionResult Do(Action action) {
			try {
				action.Invoke();
				return new ();
			} catch (Exception e) {
				return new (e);
			}
		}
		
		/// <summary> </summary>
		public static FuncResult<T> Do<T>(Func<T> func) {
			try {
				var t = func.Invoke();
				return new (t);
			} catch (Exception e) {
				return new (e);
			}
		}
		
		/// <summary> </summary>
		public static bool TryDo<T>(Func<T> func, out T result) {
			try {
				result = func();
				return true;
			} catch {
				result = default;
				return false;
			}
		}
		
		/// <summary> </summary>
		public static bool TryDo<TWith, TResult>(this TWith with, Func<TWith, TResult> whatToDo, out TResult result) {
			try {
				result = whatToDo.Invoke(with);
				return true;
			} catch {
				result = default;
				return false;
			}
		}
	}
}