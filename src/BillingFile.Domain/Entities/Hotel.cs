namespace BillingFile.Domain.Entities;

/// <summary>
/// Entity mapping to MemberPortal.dbo.Hotel table
/// Note: This is a read-only entity from existing database
/// </summary>
public class Hotel : BaseEntity
{
    // Add properties based on your actual Hotel table schema
    // These are common hotel properties - adjust based on your actual table structure
    
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    
    // Add any other properties that exist in your Hotel table
}

