using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.Devices;

public class DeviceQueryRequestDto
{
    public string? SearchTerm { get; set; }
    public DeviceType? Type { get; set; }
    public DeviceStatus? Status { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}