using AutoMapper;
using InnerHealth.Api.Dtos;
using InnerHealth.Api.Models;

namespace InnerHealth.Api.Profiles;

// Configurações do AutoMapper — define como converter Models <-> DTOs.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // UserProfile
        CreateMap<UserProfile, UserProfileDto>().ReverseMap();

        // Água
        CreateMap<WaterIntake, WaterIntakeDto>().ReverseMap();
        CreateMap<CreateWaterIntakeDto, WaterIntake>();
        CreateMap<UpdateWaterIntakeDto, WaterIntake>();

        // Exposição ao sol
        CreateMap<SunlightSession, SunlightSessionDto>().ReverseMap();
        CreateMap<CreateSunlightSessionDto, SunlightSession>();
        CreateMap<UpdateSunlightSessionDto, SunlightSession>();

        // Meditação
        CreateMap<MeditationSession, MeditationSessionDto>().ReverseMap();
        CreateMap<CreateMeditationSessionDto, MeditationSession>();
        CreateMap<UpdateMeditationSessionDto, MeditationSession>();

        // Sono
        CreateMap<SleepRecord, SleepRecordDto>().ReverseMap();
        CreateMap<CreateSleepRecordDto, SleepRecord>();
        CreateMap<UpdateSleepRecordDto, SleepRecord>();

        // Atividade física
        CreateMap<PhysicalActivity, PhysicalActivityDto>().ReverseMap();
        CreateMap<CreatePhysicalActivityDto, PhysicalActivity>();
        CreateMap<UpdatePhysicalActivityDto, PhysicalActivity>();

        // Tarefas
        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();
    }
}