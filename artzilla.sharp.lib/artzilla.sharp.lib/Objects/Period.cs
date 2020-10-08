using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ArtZilla.Net.Core.Objects {
	/// <summary>Represents a period of time</summary>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public readonly struct Period {
		public readonly DateTime Begin;
		public readonly DateTime End;

		public TimeSpan Duration => End - Begin;

		public bool IsZero => End == Begin;

		public Period(in DateTime dt1, in DateTime dt2) {
			if (dt1 < dt2) {
				Begin = dt1;
				End = dt2;
			} else {
				Begin = dt2;
				End = dt1;
			}
		}

		public Period(DateTime start, TimeSpan duration) {
			Begin = start;
			End = start.Add(duration);
		}

		/// <inheritdoc />
		public override string ToString()
			=> "[" + Begin.ToString(CultureInfo.InvariantCulture) + ", " + End.ToString(CultureInfo.InvariantCulture) + "]";
		
		public void Deconstruct(out DateTime begin, out DateTime end) {
			begin = Begin;
			end = End;
		}
		
		public void Deconstruct(out DateTime begin, out DateTime end, out TimeSpan duration) {
			begin = Begin;
			end = End;
			duration = Duration;
		}
		
		public Period MoveYears(int value = 1) => new Period(Begin.AddYears(value), Duration);
		
		public Period MoveMonths(int value = 1) => new Period(Begin.AddMonths(value), Duration);

		public Period MoveDays(double value = 1.0) => new Period(Begin.AddDays(value), Duration);

		public Period MoveHours(double value = 1.0) => new Period(Begin.AddHours(value), Duration);

		public Period MoveMinutes(double value = 1.0) => new Period(Begin.AddMinutes(value), Duration);

		public Period MoveSeconds(double value = 1.0) => new Period(Begin.AddSeconds(value), Duration);

		public static Period Today() => new Period(DateTime.Today, TimeSpan.FromDays(1.0));
		
		public static Period PreviousDay() => new Period(DateTime.Today.AddDays(-1), TimeSpan.FromDays(1.0));
		
		public static Period NextDay() => new Period(DateTime.Today.AddDays(1), TimeSpan.FromDays(1.0));
		
		public static Period CurrentMonth() {
			var today = DateTime.Today;
			var startOfTheMonth = new DateTime(today.Year, today.Month, 1);
			return new Period(startOfTheMonth, startOfTheMonth.AddMonths(1));
		}

		public static Period PreviousMonth() {
			var today = DateTime.Today;
			var startOfTheMonth = new DateTime(today.Year, today.Month, 1);
			return new Period(startOfTheMonth.AddMonths(-1), startOfTheMonth);
		}
		
		public static Period NextMonth() {
			var today = DateTime.Today;
			var startOfTheMonth = new DateTime(today.Year, today.Month, 1);
			return new Period(startOfTheMonth.AddMonths(1), startOfTheMonth.AddMonths(2));
		}
		
		public static Period CurrentYear() {
			var dt1 = new DateTime(DateTime.Today.Year, 1, 1);
			return new Period(dt1, dt1.AddYears(1));
		}

		public static Period PreviousYear() {
			var dt2 = new DateTime(DateTime.Today.Year, 1, 1);
			return new Period(dt2.AddYears(-1), dt2);
		}
		
		public static Period NextYear() {
			var dt2 = new DateTime(DateTime.Today.Year, 1, 1);
			return new Period(dt2.AddYears(-1), dt2);
		}


		public static implicit operator (DateTime Begin, DateTime End)(Period period)
			=> (period.Begin, period.End);

		public static implicit operator Period((DateTime, DateTime) dates)
			=> new Period(dates.Item1, dates.Item2);
	}
}