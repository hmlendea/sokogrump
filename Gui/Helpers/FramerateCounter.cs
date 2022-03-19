using System.Collections.Generic;
using System.Linq;

namespace SokoGrump.Gui.Helpers
{
    /// <summary>
    /// Framerate counter.
    /// </summary>
    public class FramerateCounter
    {
        static volatile FramerateCounter instance;
        static object syncRoot = new object();

        readonly Queue<float> sampleBuffer;

        /// <summary>
        /// Gets the instance of the FramerateCounter.
        /// </summary>
        /// <value>The instance.</value>
        public static FramerateCounter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new FramerateCounter();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the total number of frames.
        /// </summary>
        /// <value>The total number of frames.</value>
        public long TotalFrames { get; private set; }

        /// <summary>
        /// Gets the total seconds.
        /// </summary>
        /// <value>The total seconds.</value>
        public float TotalSeconds { get; private set; }

        /// <summary>
        /// Gets the average frames per second.
        /// </summary>
        /// <value>The average frames per second.</value>
        public float AverageFramesPerSecond { get; private set; }

        /// <summary>
        /// Gets the current frames per second.
        /// </summary>
        /// <value>The current frames per second.</value>
        public float CurrentFramesPerSecond { get; private set; }

        /// <summary>
        /// The maximum number of samples.
        /// </summary>
        public const int MAXIMUM_SAMPLES = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="FramerateCounter"/> class.
        /// </summary>
        public FramerateCounter()
        {
            sampleBuffer = new Queue<float>();
        }

        /// <summary>
        /// Updates the framerate.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public void Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                sampleBuffer.Dequeue();
                AverageFramesPerSecond = sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames += 1;
            TotalSeconds += deltaTime;
        }
    }
}
