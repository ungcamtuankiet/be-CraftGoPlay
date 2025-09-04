using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contract.DTO.CraftVillage;

namespace CGP.Contract.DTO.User
{
    public class ArtisanDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Thumbnail { get; set; }
        public ViewCraftVillageDTO? CraftVillage { get; set; }
        public ViewArtisanRequestDTO? ArtisanRequest { get; set; }
    }
}
