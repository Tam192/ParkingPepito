using CsvHelper.Configuration.Attributes;

namespace Application.Dtos.Views
{
    [Delimiter("\t")]
    [CultureInfo("InvariantCulture")]
    public class ResidentDebtsDto
    {
        [Ignore]
        public int? VehicleId { get; set; }

        [Name("Núm. placa")]
        public string? PlateNumber { get; set; }

        [Name("Tiempo estacionado (min.)")]
        public int? StayedMinutes { get; set; }

        [Ignore]
        public decimal? Cost { get; set; }

        [Name("Cantidad a pagar")]
        public decimal? TotalCost { get; set; }
    }
}
