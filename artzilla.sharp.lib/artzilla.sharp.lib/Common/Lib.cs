using System;

namespace ArtZilla.Net.Core {
	public struct ActionResult {
		public Exception Exception { get; }
		public bool IsOk => Exception == null;
		public ActionResult(Exception exception = null) { Exception = exception; }
	}

	public struct FuncResult<T> {
		public Exception Exception { get; }
		public bool IsOk => Exception == null;
		public T Result { get; }

		public FuncResult(T result) {
			Result = result;
			Exception = null;
		}
		public FuncResult(Exception exception = null) {
			Result = default(T);
			Exception = exception;
		}
	}

	public static class Lib {
		public static ActionResult Do(Action action) {
			try {
				action.Invoke();
				return new ActionResult();
			} catch (Exception e) {
				return new ActionResult(e);
			}
		}

		public static FuncResult<T> Do<T>(Func<T> func) {
			try {
				var t = func.Invoke();
				return new FuncResult<T>(t);
			} catch (Exception e) {
				return new FuncResult<T>(e);
			}
		}
	}
}