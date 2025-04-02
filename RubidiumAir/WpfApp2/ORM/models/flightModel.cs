public class Flight
{
    public int Id { get; set; }                     // Уникальный айди
    public string FlightNumber { get; set; }        // Номер рейса
    public string Destination { get; set; }         // Место назначения
    public DateTime DepartureTime { get; set; }     // Время вылета
    public DateTime ArrivalTime { get; set; }       // Время прилёта
    public string Status { get; set; }              // Статус рейса
}