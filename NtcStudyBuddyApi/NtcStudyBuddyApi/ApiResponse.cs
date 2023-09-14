namespace NtcStudyBuddyApi
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            this.Status = 1;
        }

        public object Payload { get; set; }

        public int Status { get; set; }
    }
}
