using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Contract.DTO.Quest
{
    public class CreateQuestDTO
    {
        [Required(ErrorMessage = "Tên nhiệm vụ là bắt buộc")]
        public string QuestName { get; set; }

        [Required(ErrorMessage = "Mô tả nhiệm vụ là bắt buộc")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Loại nhiệm vụ là bắt buộc")]
        public QuestType QuestType { get; set; }

        [Required(ErrorMessage = "Phần thưởng nhiệm vụ là bắt buộc")]
        public Guid Reward { get; set; }
        [Required(ErrorMessage = "Số lượng phần thưởnglà bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phẩn thưởng phải lớn hơn hoặc bằng 1.")]
        public int AmountReward { get; set; }

        [Required(ErrorMessage = "Đặt nhiệm vụ có phải hằng ngày không là bắt buộc")]
        public bool IsDaily { get; set; }
    }
}
