using AutoMapper;
using InjhinuityDiscordClient.Domain.Discord;
using InjhinuityDiscordClient.Domain.Payload.API;
using InjhinuityDiscordClient.Domain.Payload.Embed;
using InjhinuityDiscordClient.Dto;
using InjhinuityDiscordClient.Dto.Discord;
using InjhinuityDiscordClient.Dto.Embed;
using InjhinuityDiscordClient.Dto.Interfaces;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Services
{
    public class MappingService : IMappingService
    {
        private IMapper _mapper; 

        public MappingService()
        {
            InitialiseMapping();
            _mapper = Mapper.Instance;
        }

        public T Map<T>(IDiscordObjectDto data) =>
            _mapper.Map<T>(data);

        public T Map<T>(IEmbedDto data) =>
            _mapper.Map<T>(data);

        //TODO: Find a way to make this less ugly?
        public void InitialiseMapping()
        {
            Mapper.Initialize(cfg =>
            {
                #region APIObjects
                cfg.CreateMap<ChannelDto, ChannelAPIPayload>()
                    .ConstructUsing(dto => 
                        new ChannelAPIPayload(dto)
                    );

                cfg.CreateMap<CommandDto, CommandAPIPayload>()
                    .ConstructUsing(dto => 
                        new CommandAPIPayload(dto)
                    );

                cfg.CreateMap<EventDto, EventAPIPayload>()
                    .ConstructUsing(dto => 
                        new EventAPIPayload(dto)
                    );

                cfg.CreateMap<GuildDto, GuildAPIPayload>()
                    .ConstructUsing(dto => 
                        new GuildAPIPayload(dto)
                    );

                cfg.CreateMap<ParamDto, ParamAPIPayload>()
                    .ConstructUsing(dto => 
                        new ParamAPIPayload(dto)
                    );

                cfg.CreateMap<LogConfigDto, LogConfigAPIPayload>()
                    .ConstructUsing(dto => 
                        new LogConfigAPIPayload(dto)
                    );

                cfg.CreateMap<ReactionDto, ReactionAPIPayload>()
                    .ConstructUsing(dto => 
                        new ReactionAPIPayload(dto)
                    );

                cfg.CreateMap<RoleDto, RoleAPIPayload>()
                    .ConstructUsing(dto => 
                        new RoleAPIPayload(dto)
                    );

                cfg.CreateMap<UserMessageDto, UserMessageAPIPayload>()
                    .ConstructUsing(dto => 
                        new UserMessageAPIPayload(dto)
                    );

                cfg.CreateMap<MuteRoleDto, MuteRoleAPIPayload>()
                    .ConstructUsing(dto =>
                        new MuteRoleAPIPayload(dto)
                    );

                cfg.CreateMap<EventRoleDto, EventRoleAPIPayload>()
                    .ConstructUsing(dto =>
                        new EventRoleAPIPayload(dto)
                    );
                #endregion

                #region EmbedsDtos
                //EmbedDtos to actual payloads
                //Include FooterDto
                #endregion
            });
        }


    }
}
