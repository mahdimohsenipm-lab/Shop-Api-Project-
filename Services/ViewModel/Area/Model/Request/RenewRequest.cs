namespace StoreTest.Areas.Admin.Controllers
{
        public class RenewRequest
    {
            public int Id { get; set; }

            public DateTimeOffset EndTime { get; set; }

            public DateTimeOffset StartTime { get; set; }

            public int Count { get; set; }

        }
}
