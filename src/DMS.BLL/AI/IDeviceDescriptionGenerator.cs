using DMS.BLL.DTOs.AI;

namespace DMS.BLL.AI;

public interface IDeviceDescriptionGenerator
{
    Task<string> GenerateAsync(GenerateDescriptionRequestDto request, CancellationToken cancellationToken = default);
}