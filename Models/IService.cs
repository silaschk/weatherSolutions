namespace weatherSolutions.Models
{
    public interface IService
    {
        city GetForecast(string city);
    }
}
