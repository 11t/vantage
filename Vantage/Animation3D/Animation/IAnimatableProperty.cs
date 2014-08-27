namespace Vantage.Animation3D.Animation
{
    using System.Collections.Generic;

    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    public interface IAnimatableProperty<TKeyframe, TValue> : IEnumerable<TKeyframe>
        where TKeyframe : IKeyframe<TValue>
    {
        float CurrentTime { get; }

        TValue CurrentValue { get; }

        TKeyframe CurrentKeyframe { get; }

        void UpdateToTime(float time);

        void InsertKeyframe(float time, TValue value);

        void InsertKeyframe(float time, TValue value, IEasingCurve easingCurve);

        void InsertKeyframe(float time, params object[] args);

        void InsertKeyframe(IEasingCurve easingCurve, float time, params object[] args);

        void InsertKeyframe(TKeyframe keyframe);

        void InsertKeyframeAtCurrentTime(TValue value);

        void InsertKeyframeAtCurrentTime(params object[] args);

        void RemoveKeyframe(TKeyframe keyframe);

        void RemoveCurrentKeyframe();
    }
}
