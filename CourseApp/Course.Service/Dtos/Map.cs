using Course.Core.Entities;
using Course.Service.Dtos;
using Course.Service.Dtos.StudentDtos;
using System.Reflection;

public static class Mapper<TSource, TDestination>
    where TDestination : new()
{
    public static TDestination Map(TSource source)
    {
        TDestination destination = new TDestination();
        PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
        PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty = destinationProperties
                .FirstOrDefault(dp => dp.Name == sourceProperty.Name && dp.PropertyType == sourceProperty.PropertyType);

            if (destinationProperty != null)
            {
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }
        if (typeof(TSource) == typeof(Student) && typeof(TDestination) == typeof(StudentDetailsDto))
        {
            var student = source as Student;
            var studentDetailsDto = destination as StudentDetailsDto;

            if (student != null && studentDetailsDto != null)
            {
                studentDetailsDto.GroupName = student.Group?.No;
                studentDetailsDto.Birthdate = student.BirthDate;
            }
        }

        return destination;
    }
}
