using System;
using System.Text.Json.Serialization;

namespace NBT.Application.DTOs;

/// <summary>
/// Sample DTO with correct JSON serialization attributes
/// </summary>
public class SampleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("status")]
    public StatusEnum Status { get; set; }
    
    [JsonPropertyName("count")]
    public int? Count { get; set; }
}

public enum StatusEnum
{
    Pending = 0,
    Active = 1,
    Completed = 2
}
