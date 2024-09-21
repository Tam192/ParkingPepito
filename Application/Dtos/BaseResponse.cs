namespace Application.Dtos
{
    public class BaseResponse<T>
    {
        public List<T> Results { get; set; } = [];
        public List<string> Errors { get; set; } = [];
        public int Total { get; set; } = new int();

    }
}
