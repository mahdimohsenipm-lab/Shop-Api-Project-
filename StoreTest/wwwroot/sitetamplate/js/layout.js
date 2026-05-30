// js/layout.js
const layoutTemplate = {
    header: `
        <header>
            <h1>فروشگاه عشق</h1>
            <nav>
                <div id="authLinks">
                    <a href="login.html">ورود</a>
                    <a href="register.html">ثبت‌نام</a>
                </div>
                <div id="userPanel" style="display: none;">
                    <a href="#" id="logoutBtn">خروج</a>
                </div>
            </nav>
        </header>`,
    categoryNav: `
        <nav class="category-navbar">
            <div id="main-category-container" class="nav-links"></div>
        </nav>`,
    cart: `
        <section class="cart">
            <h2>🛒 سبد خرید</h2>
            <ul id="cart-items"></ul>
            <div class="cart-summary">
                <h3>مجموع: <span id="total">0 تومان</span></h3>
                <button class="button" id="checkout-btn">خرید</button>
            </div>
        </section>`
};
// تابعی برای تزریق لایوت به صفحه
function renderLayout() {
    const body = document.body;
    // ایجاد یک ظرف برای محتوای اصلی صفحه که از قبل در HTML بوده
    const mainContent = body.innerHTML;
    // بازنویسی ساختار بدنه با لایوت جدید
    body.innerHTML = `
        ${layoutTemplate.header}
        ${layoutTemplate.categoryNav}
        <main id="render-body">
            ${mainContent}
        </main>
        ${layoutTemplate.cart}
    `;
}
// اجرای تابع قبل از سایر اسکریپت‌ها
renderLayout();



//// js/layout.js
//const layoutTemplate = {
//    header: `
//        <header>
//            <h1>فروشگاه عشق</h1>
//            <nav>
//                <div id="authLinks">
//                    <a href="login.html">ورود</a>
//                    <a href="register.html">ثبت‌نام</a>
//                </div>
//                <div id="userPanel" style="display: none;">
//                    <a href="#" id="logoutBtn">خروج</a>
//                </div>
//            </nav>
//        </header>`,
//    categoryNav: `
//        <nav class="category-navbar">
//            <div id="main-category-container" class="nav-links"></div>
//        </nav>`,
//    cart: `
//        <section class="cart">
//            <h2>🛒 سبد خرید</h2>
//            <ul id="cart-items"></ul>
//            <div class="cart-summary">
//                <h3>مجموع: <span id="total">0 تومان</span></h3>
//                <button class="button checkout-btn" id="checkout-btn">خرید</button>
//            </div>
//        </section>`
//};
//function renderLayout() {
//    const body = document.body;
//    // اضافه کردن هدر و منو به "بالای" صفحه (بدون پاک کردن محتوای فعلی)
//    body.insertAdjacentHTML('afterbegin', layoutTemplate.header + layoutTemplate.categoryNav);
//    // اضافه کردن سبد خرید به "پایین" صفحه
//    body.insertAdjacentHTML('beforeend', layoutTemplate.cart);
//}
//// اجرای تابع
//renderLayout();
