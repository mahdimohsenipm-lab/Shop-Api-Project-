using StoreTest.Areas.Admin.Controllers;

namespace Services.DiscountCodeService.Area.ActivationService
{
    public interface IActivationService
    {
        Task Execute(ActivationRequest request,CancellationToken cancellationToken);
    }
}
