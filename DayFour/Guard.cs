namespace DayFour
{
    public static partial class Program
    {
        public class Guard
        {
            public Guard(int id)
            {
                Id = id;
            }

            public int Id { get; }

            public GuardState State { get; private set; } = GuardState.Awake;

            public void ApplyEvent(Event @event)
            {
                if (@event.Type == EventType.FallsAsleep)
                {
                    State = GuardState.Asleep;
                }
                else if (@event.Type == EventType.WakesUp)
                {
                    State = GuardState.Awake;
                }
            }
        }
    }
}
