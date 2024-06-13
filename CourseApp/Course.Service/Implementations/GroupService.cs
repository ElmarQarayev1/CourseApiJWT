using System;
using AutoMapper;
using Course.Core.Entities;
using Course.Data;
using Course.Data.Repostories.Interfaces;
using Course.Service.Dtos;
using Course.Service.Dtos.GroupDtos;
using Course.Service.Exceptions;
using Course.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Course.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository,IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public int Create(GroupCreateDto createDto)
        {
            if (_groupRepository.Exists(x => x.No == createDto.No && !x.IsDeleted))
            throw new RestException(StatusCodes.Status400BadRequest, "No", "No already taken");

            Group entity = new Group
            {
                No = createDto.No,
                Limit = createDto.Limit,
            };
            _groupRepository.Add(entity);
            _groupRepository.Save();

            return entity.Id;
        }
        public PaginatedList<GroupGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10)
        {
            var query = _groupRepository.GetAll(x => search == null || x.No.Contains(search), "Students");
            var paginated = PaginatedList<Group>.Create(query, page, size);
            return new PaginatedList<GroupGetDto>(_mapper.Map<List<GroupGetDto>>(paginated.Items), paginated.TotalPages, page, size);
        }

        public GroupDetailsDto GetById(int id)
        {
            Group group = _groupRepository.Get(x => x.Id == id && !x.IsDeleted,"Students");

            if (group == null) throw new RestException(StatusCodes.Status404NotFound, "Group not found");

            return _mapper.Map<GroupDetailsDto>(group);
       
        }

        public void Update(int id, GroupUpdateDto updateDto)
        {
            Group entity = _groupRepository.Get(x => x.Id == id && !x.IsDeleted, "Students");

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Group not found");

            if (entity.No != updateDto.No && _groupRepository.Exists(x => x.No == updateDto.No && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "No", "No already taken");

            if (entity.Students.Count > updateDto.Limit)
                throw new RestException(StatusCodes.Status400BadRequest, "Limit", "limit exception");

            entity.No = updateDto.No;
            entity.Limit = updateDto.Limit;
            entity.ModifiedAt = DateTime.Now;

            _groupRepository.Save();

        }

        public void Delete(int id)
        {
            Group entity = _groupRepository.Get(x => x.Id == id && !x.IsDeleted);

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Group not found");
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            _groupRepository.Save();
        }
       
   }
}

