namespace Shared.Events;

public class UpdatedPersonNameEvent
{
    public string PersonId { get; set; }
    public string NewName { get; set; }
}
