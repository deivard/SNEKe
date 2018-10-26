using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNEKe
{
    public class Trigger
    {
        int threshold;
        public bool Active = true;

        public int Threshold
        {
            set
            {
                threshold = value;
                Count = value;
            }

            get
            {
                return threshold;
            }
        }

        int Count;

        public delegate void TriggeredEventHandler();
        public event TriggeredEventHandler Triggered;

        public Trigger() : this(1) { }

        public Trigger(int threshold)
        {
            Threshold = threshold;
        }

        public void Tick()
        {
            if (Count == 0)
            {
                Count = Threshold;
                Triggered?.Invoke();
            }
            Count--;
        }

        // Invokes the triggered event and resets the trigger.
        public void ResetAndInvoke()
        {
            Count = Threshold;
            Triggered?.Invoke();
        }
    }
}

