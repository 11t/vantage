namespace Vantage.Animation3D.Animation
{
    using System.Collections.Generic;

    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    public interface IAnimatableProperty<TKeyframe, TValue> : IEnumerable<TKeyframe>
        where TKeyframe : IKeyframe<TValue>
    {
        double CurrentTime { get; }

        TValue CurrentValue { get; }

        TKeyframe CurrentKeyframe { get; }

        void UpdateToTime(double time);

        void InsertKeyframe(TKeyframe keyframe);

        void InsertKeyframe(double time, TValue value, IEasingCurve easingCurve);

        void InsertKeyframeAtCurrentTime(TValue value);

        void RemoveKeyframe(TKeyframe keyframe);

        void RemoveCurrentKeyframe();
    }
}
