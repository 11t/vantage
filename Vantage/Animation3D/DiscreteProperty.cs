using System.Collections.Generic;

namespace Vantage
{
    public class DiscreteProperty<T>
    {
        private IList<float> Times { get; set; }
        private IList<T> Values { get; set; }

        public float CurrentTime { get; set; }
        public T CurrentValue { get; set; }

        public DiscreteProperty(float initialTime, T initialValue)
        {
            Times = new List<float>();
            Values = new List<T>();
            Times.Add(initialTime);
            Values.Add(initialValue);
            CurrentTime = initialTime;
            CurrentValue = initialValue;
        }

        public void InsertKeyframe(float time, T value)
        {
            int index = IndexForTime(time) + 1;
            Times.Insert(index, time);
            Values.Insert(index, value);
        }

        public int IndexForTime(float time)
        {
            for (int i = 1; i < Times.Count; i++)
            {
                if (time < Times[i])
                {
                    return i - 1;
                }
            }
            return Times.Count - 1;
        }

        // TODO: could make faster using CurrentIndex property
        public void UpdateToTime(float time)
        {
            CurrentTime = time;
            int index = IndexForTime(time);
            if (index < 0) index = 0;
            CurrentValue = Values[index];
        }
    }
}
