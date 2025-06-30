namespace SmartMeetingAPI.DTOs
{
    public class MeetingSummaryDto
    {
        public int      Id       { get; set; }
        public string   Title    { get; set; } = default!;
        public DateTime Start    { get; set; }
        public string   RoomName { get; set; } = default!;
    }
}
