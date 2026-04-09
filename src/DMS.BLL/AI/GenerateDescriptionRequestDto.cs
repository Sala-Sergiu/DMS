namespace DMS.BLL.DTOs.AI;

public class GenerateDescriptionRequestDto
{
    public string Name { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? OperatingSystem { get; init; }
    public string? Processor { get; init; }
    public int? RamGb { get; init; }
}