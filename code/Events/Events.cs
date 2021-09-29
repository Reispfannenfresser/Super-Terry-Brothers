namespace TerryBros.Events
{
    public static class TBEvent
    {
        public static class Level
        {
            /// <summary>
            /// Triggered when a level is restarted. 
            /// </summary>
            public const string Restart = "terrybros.level.restart";

            /// <summary>
            /// Triggered when one player reached the goal. <c>TerryBrosPlayer</c> object is passed to events. 
            /// </summary>
            public const string GoalReached = "terrybros.level.goalreached";

            /// <summary>
            /// Triggered when a level is finished. 
            /// </summary>
            public const string Finished = "terrybros.level.finished";
        }
    }
}
