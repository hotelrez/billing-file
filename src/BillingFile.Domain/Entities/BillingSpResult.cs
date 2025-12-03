namespace BillingFile.Domain.Entities;

/// <summary>
/// Entity to receive results from GetBillingFileReservations stored procedure
/// This is a keyless entity - mapped to SP result set
/// Columns match the EXACT output of the stored procedure
/// </summary>
public class BillingSpResult
{
    public int ID { get; set; }
    public string? Chain_Name { get; set; }
    public int? Chain_ID { get; set; }
    public string? Hotel_Name { get; set; }
    public int? Hotel_ID { get; set; }
    public int? SAP_ID { get; set; }
    public string? Confirm_Number { get; set; }
    public string? xml { get; set; }
}

