using AutoMapper;
using Midaxo.Event.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client
{
    public static class Mappers
    {
        public static void ConfigureMapper()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Dto.SPDto.Event, Dto.Event>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Documents, opt => opt.Ignore()));
            //Mapper.Map<Dto.SPDto.Event, Dto.Event>()
            var evts = new List<Dto.SPDto.Event>();
        }

        public static IEnumerable<Dto.Event> MapEventsUsers(IEnumerable<Dto.SPDto.Event> events, IEnumerable<User> users, Guid processId)
        {
            var userDict = users.ToDictionary(
                u => u.ProcessMemberships.First(m => m.ProcessId == processId).SharepointId,
                u => u.Id);

            var resEvents = events.Select(e => new Dto.Event()
            {
                Title = e.Title,
                Type = e.Type,
                Note = e.Note,
                ReOccurance = e.ReOccurance,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Done = e.Done,
                Important = e.Important,
                AddedToOutlook = e.AddedToOutlook,
                Responsible = e.ResponsibleIds.Join(userDict, respId => respId, user => user.Key, (respId, user) => user.Value)
            });

            return resEvents;
        }

        public static IEnumerable<Dto.Event> MapProjectEvents(IEnumerable<Dto.SPDto.Event> events, MapProjectEventSettings mapSettings)
        {
            var resEvents = events.Select(e => new Dto.Event()
            {
                CustomerId = mapSettings.CustomerId,
                Title = e.Title,
                Type = e.Type,
                Note = e.Note,
                ReOccurance = e.ReOccurance,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Done = e.Done,
                Important = e.Important,
                AddedToOutlook = e.AddedToOutlook,
                Responsible = e.ResponsibleIds.Join(mapSettings.UsersDictionaty, respId => respId, user => user.Key, (respId, user) => user.Value),
                Links = (e.Tasks?.Count ?? 0) == 0 ? 
                    new List<string>() { mapSettings.ProjectPath } : 
                    e.Tasks.Join(mapSettings.Tasks, d => int.Parse(d.Key), t => t.Id, (td, task) => $"{mapSettings.ProjectPath}/{task.Guid}")
            });

            return resEvents;
        }

        public static IDictionary<int, Guid> GetProcessUsersDictionary(IEnumerable<User> users, Guid processId) =>
            users.ToDictionary(
                u => u.GetSharepointIdByProcess(processId),
                u => u.Id);
    }

    public class MapProjectEventSettings
    {
        private IDictionary<int, Guid> usersDictionaty;
        private Guid projectId;
        private string projectPath;

        public Guid CustomerId { get; set; }
        public Guid ProcessId { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Dto.SPDto.Task> Tasks { get; set; }
        public IDictionary<int, Guid> UsersDictionaty
        {
            get
            {
                if (usersDictionaty == null && ProcessId != null && Users != null)
                {
                    usersDictionaty = Mappers.GetProcessUsersDictionary(Users, ProcessId);
                }

                return usersDictionaty;
            }
        }

        public string ProjectPath
        {
            get
            {
                if (string.IsNullOrEmpty(projectPath) && CustomerId != null && ProcessId != null && ProjectId != null)
                {
                    projectPath = $"{CustomerId}/{ProcessId}/{ProjectId}";
                }

                return projectPath;
            }
        }

        public Guid ProjectId
        {
            get => projectId;
            set
            {
                projectId = value;
                projectPath = null;
            }
        }
    }
}
