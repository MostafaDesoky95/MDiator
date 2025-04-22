namespace MDiator.SampleApp.Events
{
    public class OrderCreatedEvent : IMDiatorEvent
    {
        public int OrderId { get; set; }
    }
}
