document.addEventListener("DOMContentLoaded", () => {
    // ۱. گرفتن آی‌دی محصول از آدرس صفحه
    const urlParams = new URLSearchParams(window.location.search);
    let productId = new URLSearchParams(window.location.search).get("id");
    const loadingDiv = document.getElementById('loading');
    const contentDiv = document.getElementById('content');
    let selectedRate = 5;
 

    // آدرس پایه سرور شما برای تصاویر
    const baseUrl = 'https://localhost:7061/';
    async function loadProductData() {
        if (!productId) {
            showError("شناسه محصول در آدرس یافت نشد.");
            return;
        }
        try {
            // ۲. درخواست به API
            const response = await fetch(`${baseUrl}api/Products/GetProductDetail?id=${productId}`);
            if (!response.ok) {
                throw new Error("خطا در ارتباط با سرور");
            }
            const result = await response.json();
            const p = result.data || result; // مدیریت ساختار ریسپانس
           
            renderProduct(p);
            loadComments(productId);
        } catch (err) {
            console.error("Error fetching product:", err);
            showError("خطا در دریافت اطلاعات محصول. لطفاً اتصال اینترنت یا سرور را بررسی کنید.");
        }
    }
    // ۳. تابع رندر کردن اطلاعات در صفحه
    function renderProduct(p) {
        console.log(p);
        // ===========================
        // مدیریت تصاویر
        // ===========================
        let mainImageSrc = (p.productImages && p.productImages.length > 0)
            ? `${baseUrl}${p.productImages[0].src}`
            : 'placeholder.jpg';

        let thumbnailsHtml = '';

        if (p.productImages && p.productImages.length > 0) {
            thumbnailsHtml = p.productImages.map(img => `
            <img src="${baseUrl}${img.src}"
                 alt="تصویر کوچک"
                 onclick="changeMainImage(this.src)">
        `).join('');
        }





        // ===========================
        // مشخصات فنی
        // ===========================
        let featuresHtml = (p.productFitures && p.productFitures.length > 0)
            ? p.productFitures.map(f =>
                `<tr>
                <th>${f.title}</th>
                <td>${f.description}</td>
            </tr>`
            ).join('')
            : `<tr>
                <td colspan="2" style="text-align:center;">
                    ویژگی خاصی برای این محصول ثبت نشده است.
                </td>
           </tr>`;

        // ===========================
        // موجودی
        // ===========================
        const isAvailable = p.inventory > 0;

        const inventoryHtml = isAvailable
            ? `
            <div class="inventory">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none"
                     stroke="currentColor" stroke-width="2">
                    <path d="M20 6L9 17l-5-5"/>
                </svg>
                موجود در انبار (${p.inventory} عدد)
            </div>
        `
            : `
            <div class="inventory out-of-stock">
                <svg width="18" height="18" viewBox="0 0 24 24"
                     fill="none" stroke="currentColor" stroke-width="2">
                    <circle cx="12" cy="12" r="10"/>
                    <line x1="15" y1="9" x2="9" y2="15"/>
                    <line x1="9" y1="9" x2="15" y2="15"/>
                </svg>
                ناموجود
            </div>
        `;

        // ===========================
        // دکمه خرید
        // ===========================
        const buttonHtml = isAvailable
            ? `
            <button class="btn-add-cart"
                onclick="event.stopPropagation();
                addToCart(${p.id}, '${p.name.replace(/'/g, "\\'")}', ${p.finalPrice || p.price})">

                <svg width="20" height="20"
                     viewBox="0 0 24 24"
                     fill="none"
                     stroke="currentColor"
                     stroke-width="2">
                    <circle cx="9" cy="21" r="1"/>
                    <circle cx="20" cy="21" r="1"/>
                    <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"/>
                </svg>

                افزودن به سبد خرید
            </button>
        `
            : `<button class="btn-add-cart" disabled>موجود نیست</button>`;

        // ===========================
        // قیمت و تخفیف
        // ===========================
        const hasDiscount = p.finalPrice && p.finalPrice < p.price;

        const priceHtml = hasDiscount
            ? `
            <div class="price-box">

                <div class="price-top">

                    <span class="discount-badge">
                        ${p.percentage
                ? `${p.percentage}%`
                : `${p.amount.toLocaleString()} تومان`}
                    </span>

                    <span class="old-price">
                        ${p.price.toLocaleString()} تومان
                    </span>

                </div>

                <div class="final-price">
                    ${p.finalPrice.toLocaleString()} تومان
                </div>

            </div>
        `
            : `
            <div class="price-box">

                <div class="final-price">
                    ${(p.finalPrice || p.price).toLocaleString()} تومان
                </div>

            </div>
        `;

        // ===========================
        // نمایش اطلاعات
        // ===========================
        contentDiv.innerHTML = `
        <div class="detail-grid">

            <!-- گالری تصاویر -->
            <div class="product-gallery">

                <div class="main-image-container">
                    <img src="${mainImageSrc}"
                         id="mainImg"
                         alt="${p.name}">
                </div>

                <div class="thumbnails">
                    ${thumbnailsHtml}
                </div>

            </div>

            <!-- اطلاعات محصول -->
            <div class="info-section">

                <div class="badges">
                    <span class="badge">
                        دسته بندی : ${p.category || 'عمومی'}
                    </span>

                    <span class="badge">
                        برند : ${p.brand || 'بدون برند'}
                    </span>
                </div>

                <h1>${p.name}</h1>

                <p class="description">
                    توضیحات :
                    ${p.description || 'توضیحاتی برای این محصول ثبت نشده است.'}
                </p>

                <!-- قیمت -->
                <div class="action-box">

                    <div class="price-row">
                        ${priceHtml}
                        ${inventoryHtml}
                    </div>

                    ${buttonHtml}

                </div>

                <!-- مشخصات -->
                <div class="features-section">

                    <h3>مشخصات فنی</h3>

                    <table class="features-table">
                        ${featuresHtml}
                    </table>

                </div>

            </div>

        </div>

        <!-- =======================
     Comments Section
======================== -->
<div class="comments-section">

    <div class="comments-header">

        <div class="comments-info">

            <h2>دیدگاه کاربران</h2>

            <div class="comment-summary">

                <span id="averageRate">0.0</span>

                <div id="averageStars" class="stars">
                    ☆☆☆☆☆
                </div>

                <span id="totalComments">
                    0 دیدگاه
                </span>

            </div>

        </div>

    </div>


    <!-- فرم ثبت نظر -->

    <div class="comment-form">

        <h3>ثبت دیدگاه</h3>

        <div class="rating-select">

            <span
                class="rate-star"
                data-rate="1">
                ★
            </span>

            <span
                class="rate-star"
                data-rate="2">
                ★
            </span>

            <span
                class="rate-star"
                data-rate="3">
                ★
            </span>

            <span
                class="rate-star"
                data-rate="4">
                ★
            </span>

            <span
                class="rate-star"
                data-rate="5">
                ★
            </span>

        </div>

        <textarea

            id="commentText"

            placeholder="نظر خود را بنویسید..."
            rows="5">

        </textarea>

        <button

            id="submitComment"

            class="btn-comment">

            ثبت دیدگاه

        </button>

    </div>


    <!-- لیست نظرات -->

    <div

        id="commentsContainer"

        class="comments-list">

        <div class="loading-comments">

            در حال دریافت نظرات...

        </div>

    </div>

</div>



    `;

        loadingDiv.style.display = "none";
        contentDiv.style.display = "block";
    }
    // تابع نمایش پیام خطا
    function showError(message) {
        loadingDiv.innerHTML = `<div style="color: #dc3545; font-weight: bold; padding: 20px;">${message}</div>`;
        loadingDiv.querySelector('.spinner')?.remove();
    }
    // اجرای دریافت اطلاعات

    loadProductData();

   
    


    

});
// ==========================================
// توابع عمومی (Global Functions)
// ==========================================
// تغییر عکس اصلی با کلیک روی عکس‌های کوچک
window.changeMainImage = function (src) {
    const mainImg = document.getElementById('mainImg');
    if (mainImg) {
        // ایجاد یک افکت محو شدن کوتاه هنگام تغییر عکس
        mainImg.style.opacity = '0.5';
        setTimeout(() => {
            mainImg.src = src;
            mainImg.style.opacity = '1';
        }, 150);
    }
};

//let cart = JSON.parse(localStorage.getItem("cart")) || [];
//// افزودن محصول به سبد
//function addToCart(id, name, price) {
//    const existingItem = cart.find(item => item.id === id);
//    if (existingItem) {
//        existingItem.quantity += 1;
//    } else {
//        cart.push({ id, name, price, quantity: 1 });
//    }
//    localStorage.setItem("cart", JSON.stringify(cart));
//    displayCart();
//}
//// حذف محصول
//function removeItem(id) {
//    cart = cart.filter(item => item.id !== id);
//    localStorage.setItem("cart", JSON.stringify(cart));
//    displayCart();
//}
//// تغییر تعداد
//function changeQuantity(id, change) {
//    id = Number(id);
//    const item = cart.find(product => product.id === id);
//    if (!item) return;
//    item.quantity += change;
//    if (item.quantity <= 0) {
//        cart = cart.filter(product => product.id !== id);
//    }
//    localStorage.setItem("cart", JSON.stringify(cart));
//    displayCart();
//}