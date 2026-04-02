using DMS.BLL.AI;
using DMS.BLL.DTOs.AI;
using System.ComponentModel.DataAnnotations;

namespace DMS.BLL.Services;

public sealed class DeviceDescriptionService : IDeviceDescriptionService
{
    private readonly IDeviceDescriptionGenerator _generator;

    public DeviceDescriptionService(IDeviceDescriptionGenerator generator)
    {
        _generator = generator;
    }

    public async Task<GenerateDescriptionResponseDto> GenerateAsync(
        GenerateDescriptionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var description = await _generator.GenerateAsync(request, cancellationToken);

        return new GenerateDescriptionResponseDto { Description = description };
    }

    private static void ValidateRequest(GenerateDescriptionRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Device name is required to generate a description.");

        if (string.IsNullOrWhiteSpace(request.Brand))
            throw new ValidationException("Brand is required to generate a description.");

        if (string.IsNullOrWhiteSpace(request.Model))
            throw new ValidationException("Model is required to generate a description.");

        if (string.IsNullOrWhiteSpace(request.Type))
            throw new ValidationException("Device type is required to generate a description.");
    }
}