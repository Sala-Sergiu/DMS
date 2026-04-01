using DMS.BLL.DTOs.Device;
using DMS.Domain.Entities;

namespace DMS.BLL.Mappers;

internal static class DeviceMapper
{
    internal static DeviceResponseDto ToResponseDto(Device device)
    {
        ActiveAssignmentDto? activeAssignment = null;

        if (device.ActiveAssignment is not null)
        {
            activeAssignment = new ActiveAssignmentDto
            {
                AssignmentId = device.ActiveAssignment.Id,
                UserId = device.ActiveAssignment.UserId,
                UserFullName = device.ActiveAssignment.User?.FullName ?? string.Empty,
                AssignedAtUtc = device.ActiveAssignment.AssignedAtUtc
            };
        }

        return new DeviceResponseDto
        {
            Id = device.Id,
            Name = device.Name,
            SerialNumber = device.SerialNumber,
            AssetTag = device.AssetTag,
            Brand = device.Brand,
            Model = device.Model,
            Type = device.Type,
            Status = device.Status,
            PurchasedAtUtc = device.PurchasedAtUtc,
            CreatedAtUtc = device.CreatedAtUtc,
            UpdatedAtUtc = device.UpdatedAtUtc,
            ActiveAssignment = activeAssignment
        };
    }
}