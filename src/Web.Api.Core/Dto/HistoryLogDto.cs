using System;

namespace Web.Api.Core.Dto
{
  public class HistoryLogDto
  {
    public DateTime CreatedAt { get; set; } 
    public Guid CreatedBy { get; set; } 
    public string UpdatedField { get; set; } 
    public string UpdatedState { get; set; } 
    public string PreviousState { get; set; } 
    public string Message { get; set; }
  }
}
