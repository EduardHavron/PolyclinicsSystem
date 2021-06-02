using AutoMapper;
using PolyclinicsSystemBackend.Data.Entities.Appointment;
using PolyclinicsSystemBackend.Data.Entities.Chat;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Data.Entities.User;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Doctor;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Chat;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<Diagnose, DiagnoseDto>().ReverseMap();
            CreateMap<MedicalCard, MedicalCardDto>().ReverseMap();
            CreateMap<Treatment, TreatmentDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<User, AuthorizeDto>().ReverseMap();
            CreateMap<User, DoctorDto>().ReverseMap();
        }
    }
}