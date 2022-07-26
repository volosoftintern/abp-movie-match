
using Volo.Abp;

namespace MovieMatch.Movies
{
    public class MovieAlreadyExistsException : BusinessException
    {
        public MovieAlreadyExistsException(string title):base(MovieMatchDomainErrorCodes.MovieAlreadyExists)
        {
            WithData("title", title);
        }
    }
}
