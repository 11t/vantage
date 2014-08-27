namespace Vantage.Animation3D.Animation
{
    using System;
    using System.Collections.Generic;

    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    public class AnimatableProperty<TKeyframe, TValue> : IAnimatableProperty<TKeyframe, TValue>
        where TKeyframe : class, IKeyframe<TValue>
    {
        private readonly List<TKeyframe> keyframes;
        private float currentTime;
        private int currentIndex;

        public AnimatableProperty(TValue initialValue)
        {
            this.DefaultInitialValue = initialValue;
            this.keyframes = new List<TKeyframe>();
        }

        public AnimatableProperty()
            : this(default(TValue))
        {
        }

        public TValue DefaultInitialValue { get; private set; }

        public TValue InitialValue
        {
            get
            {
                return this.keyframes.Count <= 0 ? this.DefaultInitialValue : this.keyframes[0].Value;
            }
        }

        public List<TKeyframe> Keyframes
        {
            get { return this.keyframes; }
        }

        public TKeyframe PreviousKeyframe
        {
            get { return this.CurrentIndex <= 0 ? null : this.keyframes[this.CurrentIndex - 1]; }
        }

        public TKeyframe CurrentKeyframe
        {
            get { return this.CurrentIndex < 0 ? null : this.keyframes[this.CurrentIndex]; }
        }

        public TKeyframe NextKeyframe
        {
            get { return this.CurrentIndex >= this.keyframes.Count - 1 ? null : this.keyframes[this.CurrentIndex + 1]; }
        }

        public int CurrentIndex
        {
            get
            {
                if (this.keyframes.Count <= 0)
                {
                    return -1;
                }

                return this.currentIndex;
            }

            private set
            {
                this.currentIndex = value;
            }
        }

        public float CurrentTime
        {
            get
            {
                return this.currentTime;
            }

            private set
            {
                // When CurrentTime is set, update the current keyframe index.
                this.currentTime = value;
                while (this.NextKeyframe != null && this.CurrentTime >= this.NextKeyframe.Time)
                {
                    this.CurrentIndex++;
                }

                while (this.PreviousKeyframe != null && this.CurrentTime < this.CurrentKeyframe.Time)
                {
                    this.CurrentIndex--;
                }

                // this.CurrentValue = this.keyframes[this.CurrentIndex].ValueAtTime(this.NextKeyframe, this.CurrentTime);
            }
        }

        // public TValue CurrentValue { get; private set; }
        public TValue CurrentValue
        {
            get
            {
                return this.CurrentKeyframe == null
                           ? this.InitialValue
                           : this.CurrentKeyframe.ValueAtTime(this.NextKeyframe, this.CurrentTime);
            }
        }

        public int Count
        {
            get { return this.keyframes.Count; }
        }

        public TKeyframe this[int index]
        {
            get
            {
                return this.keyframes[index];
            }

            set
            {
                this.keyframes[index] = value;
            }
        }

        public IEnumerator<TKeyframe> GetEnumerator()
        {
            return ((IEnumerable<TKeyframe>)this.keyframes).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void InsertKeyframeAtCurrentTime(TValue value)
        {
            TKeyframe keyframe = this.NewKeyframe(this.CurrentTime, value);
            if (this.CurrentIndex < 0)
            {
                this.keyframes.Insert(0, keyframe);
                this.CurrentIndex = 0;
            }

            if (this.CurrentTime == this.CurrentKeyframe.Time)
            {
                this.keyframes[this.CurrentIndex] = keyframe;
            }
            else if (this.CurrentTime < this.keyframes[0].Time)
            {
                this.keyframes.Insert(0, keyframe);
            }
            else
            {
                this.keyframes.Insert(++this.CurrentIndex, keyframe);
            }

            // this.CurrentValue = this.CurrentKeyframe.Value;
        }

        public void InsertKeyframeAtCurrentTime(params object[] args)
        {
            TValue value = this.NewValue(args);
            this.InsertKeyframeAtCurrentTime(value);
        }

        public void InsertKeyframe(float time, TValue value)
        {
            this.InsertKeyframe(this.NewKeyframe(time, value));
        }

        public void InsertKeyframe(float time, TValue value, IEasingCurve easingCurve)
        {
            this.InsertKeyframe(this.NewKeyframe(time, value, easingCurve));
        }

        public void InsertKeyframe(float time, params object[] args)
        {
            TValue value = this.NewValue(args);
            this.InsertKeyframe(time, value);
        }

        public void InsertKeyframe(IEasingCurve easingCurve, float time, params object[] args)
        {
            TValue value = this.NewValue(args);
            this.InsertKeyframe(time, value, easingCurve);
        }

        // Inserts the specified keyframe into the Keyframes list (in order) and updates the current value.
        public void InsertKeyframe(TKeyframe keyframe)
        {
            float time = keyframe.Time;

            // If Keyframes is empty or if the keyframe time is greater than the last keyframe time,
            // append the keyframe to the end of the Keyframes list.
            if (this.keyframes.Count == 0 || time > this.keyframes[this.keyframes.Count - 1].Time)
            {
                this.keyframes.Add(keyframe);
            }
            else
            {
                for (var i = 0; i < this.keyframes.Count; i++)
                {
                    if (time == this.keyframes[i].Time)
                    {
                        this.keyframes[i] = keyframe;
                        break;
                    }

                    if (time < this.keyframes[i].Time)
                    {
                        this.keyframes.Insert(i, keyframe);
                        break;
                    }
                }
            }

            // this.CurrentValue = this.CurrentKeyframe.ValueAtTime(this.NextKeyframe, this.CurrentTime);
        }

        public void RemoveKeyframe(TKeyframe keyframe)
        {
            var flag = keyframe.Time <= this.CurrentKeyframe.Time;
            this.keyframes.Remove(keyframe);
            if (flag && this.CurrentIndex > 0)
            {
                this.CurrentIndex--;
            }

            // this.CurrentValue = this.CurrentKeyframe.ValueAtTime(this.NextKeyframe, this.CurrentTime);
        }

        public void RemoveCurrentKeyframe()
        {
            this.keyframes.RemoveAt(this.CurrentIndex);
            if (this.CurrentIndex > 0)
            {
                this.CurrentIndex--;
            }

            // this.CurrentValue = this.CurrentKeyframe.ValueAtTime(this.NextKeyframe, this.CurrentTime);
        }

        public void UpdateToTime(float time)
        {
            this.CurrentTime = time;
        }

        private TKeyframe NewKeyframe(float time, TValue value, IEasingCurve easingCurve)
        {
            TKeyframe keyframe = Activator.CreateInstance(typeof(TKeyframe), time, value, easingCurve) as TKeyframe;
            return keyframe;
        }

        private TKeyframe NewKeyframe(float time, TValue value)
        {
            return this.NewKeyframe(time, value, BasicEasingCurve.Linear);
        }

        private TValue NewValue(params object[] args)
        {
            TValue value = (TValue)Activator.CreateInstance(typeof(TValue), args);
            return value;
        }
    }
}
