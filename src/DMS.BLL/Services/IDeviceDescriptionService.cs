using DMS.BLL.DTOs.AI;

namespace DMS.BLL.Services;

public interface IDeviceDescriptionService
{
    Task<GenerateDescriptionResponseDto> GenerateAsync(GenerateDescriptionRequestDto request, CancellationToken cancellationToken = default);
}