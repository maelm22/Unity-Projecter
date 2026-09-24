using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalClock
{
    class ClockDisplay
    {
        NumberDisplay hours;
        NumberDisplay minutes;
        public string displayString;

        public ClockDisplay()
        {
            hours = new NumberDisplay(24);
            minutes = new NumberDisplay(60);
            UpdateDisplay();
        }

        public ClockDisplay(int hour, int minute)
        {
            hours = new NumberDisplay(24);
            minutes = new NumberDisplay(60);
            SetTime(hour, minute);
        }

        public void SetTime(int hour, int minute)
        {
            hours.SetValue(hour);
            minutes.SetValue(minute);
            
        }

        void UpdateDisplay()
        {
            displayString = hours.GetDisplayValue() + ":" + minutes.GetDisplayValue();
        }

        public void TimeTick()
        {
            UpdateDisplay();
            minutes.Increment();
            if (minutes.GetValue() == 0)
                hours.Increment();
            
        }
    }
    class NumberDisplay
    {
        private int limit;
        private int value;

        public NumberDisplay(int rolloverLimit)
        {
            limit = rolloverLimit;
            value = 0;
        }

        public void Increment()
        {
            value++;
            value = value % limit;
        }

        public string GetDisplayValue()
        {
            if (value < 10)
                return "0" + value;
            else
                return value.ToString();
        }

        public void SetValue(int replacementValue)
        {
            if (replacementValue >= 0 && replacementValue <= limit)
                value = replacementValue;
        }

        public int GetValue()
        {
            return value;
        }
    }
}
