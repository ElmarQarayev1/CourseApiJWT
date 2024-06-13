using System;
using Course.Service.Dtos;
using Course.Service.Dtos.GroupDtos;

namespace Course.Service.Interfaces
{
	public interface IGroupService
	{
        int Create(GroupCreateDto createDto);
        PaginatedList<GroupGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10);
        GroupDetailsDto GetById(int id);
        void Update(int id, GroupUpdateDto updateDto);
        void Delete(int id);
    }
}

