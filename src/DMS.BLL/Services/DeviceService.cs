using DMS.BLL.DTOs.Device;
using DMS.BLL.Exceptions;
using DMS.BLL.Mappers;
using DMS.DAL.Repositories;
using DMS.DAL.UnitOfWork;
using DMS.Domain.Entities;
using DMS.Domain.Enums;

namespace DMS.BLL.Services;

public class DeviceService : IDeviceService
{
    private readonly IUnitOfWork _uow;

    public DeviceService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResponseDto<DeviceResponseDto>> GetPagedAsync(
        DeviceListRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var parameters = new DeviceQueryParameters
        {
            SearchTerm = request.SearchTerm,
            Type = request.Type,
            Status = request.Status,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var (items, totalCount) = await _uow.Devices.GetPagedAsync(parameters, cancellationToken);

        return new PagedResponseDto<DeviceResponseDto>
        {
            Items = items.Select(DeviceMapper.ToResponseDto).ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<DeviceResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var device = await _uow.Devices.GetWithAssignmentsAsync(id, cancellationToken)
            ?? throw NotFoundException.For<Device>(id);

        return DeviceMapper.ToResponseDto(device);
    }

    public async Task<DeviceResponseDto> CreateAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        if (await _uow.Devices.IsSerialNumberTakenAsync(request.SerialNumber, cancellationToken: cancellationToken))
            throw new ConflictException($"Serial number '{request.SerialNumber}' is already in use.");

        if (await _uow.Devices.IsAssetTagTakenAsync(request.AssetTag, cancellationToken: cancellationToken))
            throw new ConflictException($"Asset tag '{request.AssetTag}' is already in use.");

        var device = new Device(
            request.Name,
            request.SerialNumber,
            request.AssetTag,
            request.Brand,
            request.Model,
            request.Type,
            request.OperatingSystem,
            request.OsVersion,
            request.Processor,
            request.RamGb,
            request.Description,
            request.PurchasedAtUtc);

        await _uow.Devices.AddAsync(device, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return DeviceMapper.ToResponseDto(device);
    }

    public async Task<DeviceResponseDto> UpdateAsync(
        Guid id,
        UpdateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateUpdateRequest(request);

        var device = await _uow.Devices.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<Device>(id);

        device.UpdateDetails(
            request.Name, request.Brand, request.Model, request.Type,
            request.OperatingSystem, request.OsVersion, request.Processor,
            request.RamGb, request.Description);

        _uow.Devices.Update(device);
        await _uow.SaveChangesAsync(cancellationToken);

        return DeviceMapper.ToResponseDto(device);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var device = await _uow.Devices.GetWithAssignmentsAsync(id, cancellationToken)
            ?? throw NotFoundException.For<Device>(id);

        if (device.IsAssigned)
            throw new ConflictException("Cannot delete a device that is currently assigned.");

        // Şterge assignment-urile istorice înainte de device
        await _uow.Devices.DeleteAssignmentsAsync(id, cancellationToken);

        _uow.Devices.Delete(device);
        await _uow.SaveChangesAsync(cancellationToken);
    }

    public async Task<DeviceResponseDto> AssignToSelfAsync(
        Guid deviceId, Guid currentUserId, string? notes,
        CancellationToken cancellationToken = default)
    {
        var device = await _uow.Devices.GetWithAssignmentsAsync(deviceId, cancellationToken)
            ?? throw NotFoundException.For<Device>(deviceId);

        var user = await _uow.Users.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw NotFoundException.For<User>(currentUserId);

        if (device.IsAssigned)
            throw new ConflictException("Device is already assigned to another user.");

        if (device.Status == DeviceStatus.Retired)
            throw new ConflictException("Cannot assign a retired device.");

        var assignment = new DeviceAssignment(device, user, notes);
        device.MarkAsInUse();

        await _uow.Devices.AddAssignmentAsync(assignment, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return DeviceMapper.ToResponseDto(device);
    }

    public async Task<DeviceResponseDto> UnassignFromSelfAsync(
        Guid deviceId, Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        var device = await _uow.Devices.GetWithAssignmentsAsync(deviceId, cancellationToken)
            ?? throw NotFoundException.For<Device>(deviceId);

        if (!device.IsAssigned)
            throw new ConflictException("Device is not currently assigned.");

        if (device.ActiveAssignment!.UserId != currentUserId)
            throw new ConflictException("You can only unassign a device assigned to yourself.");

        device.ActiveAssignment.Return();
        device.MarkAsAvailable();

        await _uow.SaveChangesAsync(cancellationToken);

        return DeviceMapper.ToResponseDto(device);
    }

    private static void ValidateCreateRequest(CreateDeviceRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Device name is required.");
        if (string.IsNullOrWhiteSpace(request.SerialNumber))
            throw new ValidationException("Serial number is required.");
        if (string.IsNullOrWhiteSpace(request.AssetTag))
            throw new ValidationException("Asset tag is required.");
        if (string.IsNullOrWhiteSpace(request.Brand))
            throw new ValidationException("Brand is required.");
        if (string.IsNullOrWhiteSpace(request.Model))
            throw new ValidationException("Model is required.");
    }

    private static void ValidateUpdateRequest(UpdateDeviceRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Device name is required.");
        if (string.IsNullOrWhiteSpace(request.Brand))
            throw new ValidationException("Brand is required.");
        if (string.IsNullOrWhiteSpace(request.Model))
            throw new ValidationException("Model is required.");
    }
}