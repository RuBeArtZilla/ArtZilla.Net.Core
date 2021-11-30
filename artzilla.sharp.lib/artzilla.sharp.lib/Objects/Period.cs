using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ArtZilla.Net.Core;

public interface IReadOnlyPeriod {
	DateTime Begin { get; }
	DateTime End { get; }
}

/// <summary>Represents a period of time</summary>
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly struct Period : IReadOnlyPeriod, IEquatable<Period> {
	public readonly DateTime Begin;
	public readonly DateTime End;

	/// <inheritdoc />
	DateTime IReadOnlyPeriod.Begin => Begin;

	/// <inheritdoc />
	DateTime IReadOnlyPeriod.End => End;

	/// timespan between Begin and End
	public TimeSpan Duration() => End - Begin;

	/// zero duration
	public bool IsZero() => End == Begin;

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

	public Period Move(TimeSpan shift) => this + shift;

	public Period MoveYears(int value = 1) => new(Begin.AddYears(value), End.AddYears(value));

	public Period MoveMonths(int value = 1) => new(Begin.AddMonths(value), End.AddMonths(value));

	public Period MoveDays(double value = 1.0) => new(Begin.AddDays(value), End.AddDays(value));

	public Period MoveHours(double value = 1.0) => new(Begin.AddHours(value), End.AddHours(value));

	public Period MoveMinutes(double value = 1.0) => new(Begin.AddMinutes(value), End.AddMinutes(value));

	public Period MoveSeconds(double value = 1.0) => new(Begin.AddSeconds(value), End.AddSeconds(value));

	public static Period Today() => new(DateTime.Today, TimeSpan.FromDays(1.0));

	public static Period PreviousDay() => new(DateTime.Today.AddDays(-1), TimeSpan.FromDays(1.0));

	public static Period NextDay() => new(DateTime.Today.AddDays(1), TimeSpan.FromDays(1.0));

	public static Period CurrentMonth() {
		var today = DateTime.Today;
		var startOfTheMonth = new DateTime(today.Year, today.Month, 1);
		return new(startOfTheMonth, startOfTheMonth.AddMonths(1));
	}

	public static Period PreviousMonth() {
		var today = DateTime.Today;
		var startOfTheMonth = new DateTime(today.Year, today.Month, 1);
		return new(startOfTheMonth.AddMonths(-1), startOfTheMonth);
	}

	public static Period NextMonth() {
		var today = DateTime.Today;
		var startOfTheMonth = new DateTime(today.Year, today.Month, 1);
		return new(startOfTheMonth.AddMonths(1), startOfTheMonth.AddMonths(2));
	}

	public static Period CurrentYear() {
		var dt1 = new DateTime(DateTime.Today.Year, 1, 1);
		return new(dt1, dt1.AddYears(1));
	}

	public static Period PreviousYear() {
		var dt2 = new DateTime(DateTime.Today.Year, 1, 1);
		return new(dt2.AddYears(-1), dt2);
	}

	public static Period NextYear() {
		var dt2 = new DateTime(DateTime.Today.Year, 1, 1);
		return new(dt2.AddYears(-1), dt2);
	}

	/// <inheritdoc />
	public bool Equals(Period other)
		=> Begin.Equals(other.Begin) && End.Equals(other.End);

	/// <inheritdoc />
	public override bool Equals(object obj)
		=> obj is Period other && Equals(other);

	/// <inheritdoc />
	public override int GetHashCode() {
		unchecked { return (Begin.GetHashCode() * 397) ^ End.GetHashCode(); }
	}

	public static bool operator ==(Period left, Period right)
		=> left.Equals(right);

	public static bool operator !=(Period left, Period right)
		=> !left.Equals(right);

	public void Deconstruct(out DateTime begin, out DateTime end) {
		begin = Begin;
		end = End;
	}

	public static implicit operator (DateTime Begin, DateTime End)(Period period)
		=> (period.Begin, period.End);

	public static implicit operator Period((DateTime, DateTime) dates)
		=> new(dates.Item1, dates.Item2);

	public static Period operator +(Period period, TimeSpan shift)
		=> new(period.Begin + shift, period.End + shift);

	public static TimeSpan operator +(TimeSpan duration, Period period)
		=> duration + period.Duration();

	public static Period operator -(Period period, TimeSpan shift)
		=> new(period.Begin - shift, period.End - shift);

	public static TimeSpan operator -(TimeSpan duration, Period period)
		=> duration - period.Duration();
}
