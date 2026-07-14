using System.Collections.Generic;
using System.Linq;

namespace SokoGrump.Gui.Helpers
{
    /// <summary>
    /// Framerate counter.
    /// </summary>
    public sealed class FramerateCounter : Singleton<FramerateCounter>
    {
        private static int MaximumSamples => 100;

        private readonly Queue<float> sampleBuffer = new Queue<float>();

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
        /// Updates the framerate.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public void Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (sampleBuffer.Count > MaximumSamples)
            {
                sampleBuffer.Dequeue();
                AverageFramesPerSecond = sampleBuffer.Average(sample => sample);
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
