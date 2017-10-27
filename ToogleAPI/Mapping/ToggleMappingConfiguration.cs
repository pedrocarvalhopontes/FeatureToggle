using System;
using AutoMapper;
using ToogleAPI.Models;

namespace ToggleAPI.Mapping
{
    /// <summary>
    /// Responsible for the configuration of the Mapping between the DTO's and the Entity classes.
    /// </summary>
    public class ToggleMappingConfiguration
    {
        public IConfigurationProvider Configure()
        {
            return new MapperConfiguration(ConfigurationAction);
        }

        public Action<IMapperConfigurationExpression> ConfigurationAction
        {
            get
            {
                return cfg =>
                {
                    cfg.CreateMap<Toggle, ToggleDtoOutput>(MemberList.Destination);
                    cfg.CreateMap<Toggle, ToggleDtoInput>(MemberList.Destination);
                    cfg.CreateMap<ToggleDtoInput, Toggle>(MemberList.Source);

                    cfg.CreateMap<Configuration, ConfigurationDtoOutput>(MemberList.Destination);
                    cfg.CreateMap<Configuration, ConfigurationDtoInput>(MemberList.Destination);
                    cfg.CreateMap<ConfigurationDtoInput, Configuration>(MemberList.Source);
                };
            }
        }
    }
}
