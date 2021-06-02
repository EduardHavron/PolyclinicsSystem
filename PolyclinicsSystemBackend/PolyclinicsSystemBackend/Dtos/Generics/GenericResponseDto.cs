using System.Collections.Generic;

namespace PolyclinicsSystemBackend.Dtos.Generics
{
    public class GenericResponse<T, TE>
    {
        public bool IsSuccess { get; set; }
        
        public IEnumerable<T>? Errors { get; set; }
        
        public TE? Result { get; set; }
    }
}