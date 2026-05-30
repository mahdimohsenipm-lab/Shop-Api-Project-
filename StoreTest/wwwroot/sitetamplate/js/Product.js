// این تابع فقط مسئول انتقال کاربر به صفحه جدید است
function showProductDetail(productId) {
    // ارسال آی‌دی محصول از طریق URL به صفحه جدید
    window.location.href = `product-detail.html?id=${productId}`;
}
