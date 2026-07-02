using StoreTest.Areas.Admin.Controllers;

namespace Services.DiscountCodeService.Area.ActivationService
{
    public interface IActivationService
    {
        Task Execute(RenewRequest request,CancellationToken cancellationToken);
    }
}
