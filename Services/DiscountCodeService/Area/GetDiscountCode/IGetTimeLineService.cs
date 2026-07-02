using Services.ViewModel.Area.Model.Dto;

namespace Services.DiscountCodeService.Area.GetDiscountCode
{
    public interface IGetTimeLineService
    {
        TimeLine Execute(DiscountCodeDto request);
    }

    public class GetTimeLineService : IGetTimeLineService
    {
        public TimeLine Execute(DiscountCodeDto request)
        {
            var now = DateTimeOffset.Now;

            // اولویت وضعیت‌ها
            //if (!request.IsActive)
            //    return Disabled(request, now);
            if (!request.IsActive)
            {
                return NotActive(request, now);
            }
            if (request.Count <= 0)
                return Finished(request, now);

            if (now < request.StartTime)
                return NotStarted(request, now);

            if (now > request.EndTime)
                return Expired(request, now);

            return Active(request, now);
        }

        private TimeLine Active(DiscountCodeDto request, DateTimeOffset now)
        {
            return new TimeLine
            {
                Status = DiscountCodeStatus.Active,
                StatusText = "کد تخفیف فعال است",

                RemainingTime = request.EndTime - now,
                RemainingTimeText = "زمان باقی مانده تا پایان",

                IsStarted = true,
                IsExpired = false
            };
        }


        private TimeLine NotActive(DiscountCodeDto request, DateTimeOffset now)
        {
            return new TimeLine
            {
                Status = DiscountCodeStatus.Disabled,
                StatusText = "کد تخفیف غیرفعال است",

                RemainingTime = request.EndTime - now,
                RemainingTimeText = "زمان باقی مانده تا پایان",

                IsStarted = true,
                IsExpired = false
            };
        }

        private TimeLine NotStarted(DiscountCodeDto request, DateTimeOffset now)
        {
            return new TimeLine
            {
                Status = DiscountCodeStatus.NotStarted,
                StatusText = "زمان کد تخفیف شروع نشده است",

                RemainingTime = request.StartTime - now,
                RemainingTimeText = "زمان باقی مانده تا شروع",

                IsStarted = false,
                IsExpired = false
            };
        }

        private TimeLine Expired(DiscountCodeDto request, DateTimeOffset now)
        {
            return new TimeLine
            {
                Status = DiscountCodeStatus.Expired,
                StatusText = "کد تخفیف منقضی شده است",

                RemainingTime = TimeSpan.Zero,
                RemainingTimeText = "زمان کد تخفیف به پایان رسیده است",

                IsStarted = true,
                IsExpired = true
            };
        }

        private TimeLine Finished(DiscountCodeDto request, DateTimeOffset now)
        {
            return new TimeLine
            {
                Status = DiscountCodeStatus.Finished,
                StatusText = "تعداد کد تخفیف تمام شده است",

                RemainingTime = now < request.StartTime
                    ? request.StartTime - now
                    : TimeSpan.Zero,

                RemainingTimeText = now < request.StartTime
                    ? "زمان باقی مانده تا شروع"
                    : "اتمام ظرفیت",

                IsStarted = request.StartTime <= now,
                IsExpired = request.EndTime <= now
            };
        }

        //private TimeLine Disabled(TimeLineRequest request, DateTimeOffset now)
        //{
        //    var result = new TimeLine
        //    {
        //        Status = DiscountCodeStatus.Disabled,
        //        StatusText = "کد تخفیف غیرفعال شده است",

        //        IsStarted = request.StartTime <= now,
        //        IsExpired = request.EndTime <= now
        //    };

        //    if (now < request.StartTime)
        //    {
        //        result.RemainingTime = request.StartTime - now;
        //        result.RemainingTimeText = "زمان باقی مانده تا شروع";
        //    }
        //    else if (now > request.EndTime)
        //    {
        //        result.RemainingTime = TimeSpan.Zero;
        //        result.RemainingTimeText = "زمان کد تخفیف به پایان رسیده است";
        //    }
        //    else
        //    {
        //        result.RemainingTime = request.EndTime - now;
        //        result.RemainingTimeText = "زمان باقی مانده تا پایان";
        //    }

        //    return result;
        //}
    }
}

    public class TimeLineRequest
    {
        public DateTimeOffset EndTime { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public int Count { get; set; }

      

    }
    public class TimeLine
    {
        public DiscountCodeStatus Status { get; set; }
        public string StatusText { get; set; }
        public TimeSpan? RemainingTime { get; set; }
        public string RemainingTimeText { get; set; }
        public bool IsStarted { get; set; }
        public bool IsExpired { get; set; }

    }


