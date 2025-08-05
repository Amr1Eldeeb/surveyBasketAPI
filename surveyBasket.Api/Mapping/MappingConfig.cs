using System.Runtime.InteropServices;

namespace surveyBasket.Api.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            ////config.NewConfig<Poll, PollResponse>()
            ////   .Map(dest => dest.Notes, src => src.Description);
            //config.NewConfig<Student, StudentResponse>()
            //    .Map(dest => dest.FullName, src => $"{src.FirstName} {src.MiddleName} {src.LastName}")
            //    .Map(dest => dest.Agee, src => DateTime.Now.Year - src.DateOfBirth!.Value.Year,
            //    srcCond => srcCond.DateOfBirth.HasValue);// is a  validation
        }
    }
}
