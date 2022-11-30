using System;

namespace ArtZilla.Net.Core;

/// 
public struct ActionResult {
	/// 
	public Exception Exception { get; }

	/// 
	public bool IsOk => Exception == null;

	/// 
	public ActionResult(Exception exception = null) => Exception = exception;
}


/// 
public struct FuncResult<T> {
	/// 
	public Exception Exception { get; }
	/// 
	public bool IsOk => Exception == null;
	/// 
	public T Result { get; }

	/// 
	public FuncResult(T result) {
		Result = result;
		Exception = null;
	}

	/// 
	public FuncResult(Exception exception = null) {
		Result = default;
		Exception = exception;
	}
}

/// 
public static class Lib {
	/// 
	public static ActionResult Do(Action action) {
		try {
			action.Invoke();
			return new();
		} catch (Exception e) {
			return new(e);
		}
	}

	/// 
	public static FuncResult<T> Do<T>(Func<T> func) {
		try {
			var t = func.Invoke();
			return new(t);
		} catch (Exception e) {
			return new(e);
		}
	}

	/// 
	public static bool TryDo<T>(Func<T> func, out T result) {
		try {
			result = func();
			return true;
		} catch {
			result = default;
			return false;
		}
	}

	/// 
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
