namespace InjhinuityDiscordClient.Domain.Payload.API
{
    public interface IAPIPayload : IPayload
    {
        string ToGetAllAPIString();
        string ToGetAPIString();
        string ToPostAPIString();
        string ToPutAPIString();
        string ToDeleteAPIString();
        string ToJson();
    }
}
