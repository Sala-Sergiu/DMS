using DMS.BLL.DTOs.Assignment;
using DMS.BLL.DTOs.Device;

namespace DMS.BLL.Services;

public interface IDeviceService
{
    Task<PagedResponseDto<DeviceResponseDto>> GetPagedAsync(DeviceListRequestDto request, CancellationToken cancellationToken = default);
    Task<DeviceResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DeviceResponseDto> CreateAsync(CreateDeviceRequestDto request, CancellationToken cancellationToken = default);
    Task<DeviceResponseDto> UpdateAsync(Guid id, UpdateDeviceRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DeviceResponseDto> AssignAsync(Guid deviceId, AssignDeviceRequestDto request, CancellationToken cancellationToken = default);
    Task<DeviceResponseDto> ReturnAsync(Guid deviceId, ReturnDeviceRequestDto request, CancellationToken cancellationToken = default);
}