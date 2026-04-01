using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.Device;

public class DeviceListRequestDto
{
    public string? SearchTerm { get; init; }
    public DeviceType? Type { get; init; }
    public DeviceStatus? Status { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}