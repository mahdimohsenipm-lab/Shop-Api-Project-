using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModel.Area.Model.Dto
{
    public class DiscountCodeDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int Count { get; set; }

        public int LimitPrice { get; set; }

        public int? Amount { get; set; }

        public int? Percentage { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        // وضعیت محاسبه شده
        public DiscountCodeStatus Status { get; set; }

        // متن آماده برای نمایش در UI
        public string StatusText { get; set; }

        // زمان باقی مانده تا شروع یا پایان
        public TimeSpan? RemainingTime { get; set; }

        // برای نمایش راحت در UI
        public string RemainingTimeText { get; set; }

        // فلگ های کمکی
        public bool IsStarted { get; set; }

        public bool IsExpired { get; set; }



        public void Apply(TimeLine state)
        {
            Status = state.Status;
            StatusText = state.StatusText;
            RemainingTime = state.RemainingTime;
            RemainingTimeText = state.RemainingTimeText;
            IsStarted = state.IsStarted;
            IsExpired = state.IsExpired;
        }
    }

    public enum DiscountCodeStatus
    {

        NotStarted = 1, // هنوز شروع نشده
        Active = 2,     // فعال
        Expired = 3,    // منقضی شده
        Disabled = 4,   // دستی غیرفعال شده
        Finished = 5

    }
}
