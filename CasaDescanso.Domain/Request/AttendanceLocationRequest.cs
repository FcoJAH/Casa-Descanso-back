namespace CasaDescanso.Domain.Requests
{
    public class AttendanceLocationRequest 
    {
        public int UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}