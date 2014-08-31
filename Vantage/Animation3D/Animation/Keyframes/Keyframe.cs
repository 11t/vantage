// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Keyframe.cs" company="Tea Party Productions">
//   (c) 2014 Tea Party Productions.
// </copyright>
// <summary>
//   Represents an animation keyframe.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Vantage.Animation3D.Animation.Keyframes
{
    using Vantage.Animation3D.Animation.EasingCurves;

    /// <summary>
    /// Represents an animation keyframe.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value of the keyframe.
    /// </typeparam>
    public class Keyframe<T> : IKeyframe<T>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Keyframe{T}"/> class.
        /// </summary>
        /// <param name="time">
        /// The time to initialize the keyframe.
        /// </param>
        /// <param name="value">
        /// The value of the keyframe at its specified time.
        /// </param>
        public Keyframe(double time, T value)
            : this(time, value, BasicEasingCurve.Linear)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keyframe{T}"/> class.
        /// </summary>
        /// <param name="time">
        /// The time to initialize the keyframe.
        /// </param>
        /// <param name="value">
        /// The value of the keyframe at its specified time.
        /// </param>
        /// <param name="easingCurve">
        /// The easing curve that controls how values are interpolated between this keyframe and the next keyframe.
        /// </param>
        public Keyframe(double time, T value, IEasingCurve easingCurve)
        {
            this.Time = time;
            this.Value = value;
            this.EasingCurve = easingCurve;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the easing curve.
        /// </summary>
        public IEasingCurve EasingCurve { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        public double Time { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Calculates the interpolated value between this Keyframe and the specified next Keyframe, at the given time.
        /// </summary>
        /// <param name="next">
        /// The next Keyframe. The next Keyframe should have a Time greater than than the Time of this Keyframe.
        /// </param>
        /// <param name="time">
        /// The time at which the value is desired.
        /// </param>
        /// <returns>
        /// The type <see cref="T"/> value representing the interpolation between this Keyframe and the next Keyframe at the
        ///     given time.
        /// </returns>
        public T ValueAtTime(IKeyframe<T> next, double time)
        {
            if (time <= this.Time || this.EasingCurve == null || next == null)
            {
                return this.Value;
            }

            double x = (time - this.Time) / (next.Time - this.Time);
            double y = this.EasingCurve.Evaluate(x);
            return this.Interpolate(this.Value, next.Value, y);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Represents the calculation performed when interpolating between two values of type T by the given amount. Override
        ///     this method to implement interpolation methods that are different from the default linear interpolation.
        /// </summary>
        /// <param name="start">
        /// The start T value.
        /// </param>
        /// <param name="end">
        /// The end T value.
        /// </param>
        /// <param name="amount">
        /// The amount to interpolate by. 0 represents that the returned value should equal start, and 1 represents that the
        ///     returned value should equal end.
        /// </param>
        /// <returns>
        /// The type <see cref="T"/> value representing an interpolation between start and end by amount.
        /// </returns>
        internal virtual T Interpolate(T start, T end, double amount)
        {
            dynamic a = start;
            dynamic b = end;
            float floatAmount = (float)amount;
            return (a * (1 - floatAmount)) + (b * floatAmount);
        }

        #endregion
    }
}