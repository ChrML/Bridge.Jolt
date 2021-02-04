namespace Bridge.Jolt
{
    /// <summary>
    /// Provides information about the state of a pending task.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// There is no information about the state.
        /// </summary>
        None,

        /// <summary>
        /// The pending task is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The pending task has completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The pending task completed with an error.
        /// </summary>
        Error
    }
}
