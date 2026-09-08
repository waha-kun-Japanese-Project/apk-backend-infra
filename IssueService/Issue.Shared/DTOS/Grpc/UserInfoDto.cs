namespace GrpcUserClient.DTOS
{
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }
}
