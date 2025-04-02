public class Baggage
{
    public string Id { get; set; }                  // Номер багажа
    public string PassengerId { get; set; }         // Айди пассажира
    public double Weight { get; set; }              // Вес 
    public string Status { get; set; }              // Статус багажа
    public int FlightId { get; set; }               // Рейс к которому привязан
    public Flight Flight { get; set; }              // Ссылка на рейс
}