using System.ComponentModel.DataAnnotations;

namespace Order.API.Models.Entities;

public class OrderOutbox
{
	[Key]
	public Guid IdempotentToken { get; set; }
	public DateTime OccuredOn { get; set; } // event'ın gerçekleşme tarihi
	public DateTime? ProcessedDate { get; set; } // event'ın işlenme tarihi
	public string Type { get; set; } = default!; // hangi event?
	public string Payload { get; set; } = default!; // event'ın içeriği
}
