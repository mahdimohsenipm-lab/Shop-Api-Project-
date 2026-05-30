//// ۱. تابع دریافت و نمایش محصولات (با همان استایلی که خواستی)
//async function fetchProducts(categoryId) {
//    const productContainer = document.getElementById('product-list'); // آیدی محلی که محصولات باید نمایش داده شوند
//    // نمایش حالت لودینگ (اختیاری)
//    productContainer.innerHTML = '<p>در حال بارگذاری محصولات...</p>';
//    try {
//        const response = await fetch(`https://localhost:7061/api/Category/GetProducts?catid=${categoryId}`);
//        const result = await response.json();
//        console.log(result);

//        // فرض می‌کنیم دیتای محصولات در result.data یا خود result است
//        const products = result.data || result;
//        if (products && products.length > 0) {
//            productContainer.innerHTML = products.map(product => `
//                <div class="product-card">
//                    <div class="product-image">
//                        <img src="https://localhost:7061/${product.src}" />
//                    </div>
//                    <div class="product-info">
//                        <h3>${product.name}</h3>
//                        <p class="price">
//                            ${product.price.toLocaleString()} تومان
//                        </p>
//                        <button onclick="addToCart(${product.id}, '${product.name.replace(/'/g, "\\'")}', ${product.price})">
//                            افزودن به سبد
//                        </button>
//                    </div>
//                </div>
//            `).join('');
//        } else {
//            productContainer.innerHTML = '<p>محصولی در این دسته‌بندی یافت نشد.</p>';
//        }
//    } catch (err) {
//        console.error("خطا در دریافت محصولات:", err);
//        productContainer.innerHTML = '<p>خطا در برقراری ارتباط با سرور.</p>';
//    }
//}
//// ۲. اصلاح تابع رندر نوار منو
//async function renderNavbar() {
//    try {
//        const response = await fetch('https://localhost:7061/api/Category/Index');
//        const result = await response.json();
//        if (result.isSuccess) {
//            const container = document.getElementById('main-category-container');
//            if (!container) return;
//            container.className = "ok-cat-navbar";
//            container.innerHTML = '';
//            const mainTriggerHtml = `
//                <div class="ok-item">
//                    <a href="#" class="ok-cat-nav-item ok-cat-bold">
//                        <i class="icon-menu"></i>
//                        <span>دسته‌بندی کالاها</span>
//                    </a>
//                </div>
//                <div class="ok-cat-divider"></div>
//            `;
//            container.insertAdjacentHTML('beforeend', mainTriggerHtml);
//            result.data.forEach(cat => {
//                const wrapper = document.createElement('div');
//                wrapper.className = 'ok-item';
//                // تغییر مهم: استفاده از onclick به جای لینک مستقیم
//                let itemContent = `
//                    <a href="javascript:void(0)" onclick="fetchProducts(${cat.id})" class="ok-cat-nav-item">
//                        <span>${cat.name}</span>
//                    </a>
//                `;
//                if (cat.hasChildern && cat.subCategory && cat.subCategory.length > 0) {
//                    itemContent += `<ul class="ok-dropdown">`;
//                    cat.subCategory.forEach(sub => {
//                        itemContent += `
//                            <li>
//                                <a href="javascript:void(0)" onclick="fetchProducts(${sub.id})">${sub.name}</a>
//                            </li>
//                        `;
//                    });
//                    itemContent += `</ul>`;
//                }
//                wrapper.innerHTML = itemContent;
//                container.appendChild(wrapper);
//            });
//        }
//    } catch (err) {
//        console.error("خطا در لود منو:", err);
//    }
//}
//document.addEventListener('DOMContentLoaded', renderNavbar);


// تابع کمکی برای مدیریت استایل آیتم انتخاب شده
function setActiveCategory(element) {
    // حذف کلاس active از تمام لینک‌های منو
    const allItems = document.querySelectorAll('.ok-cat-nav-item, .ok-dropdown a');
    allItems.forEach(item => item.classList.remove('active'));
    // اضافه کردن کلاس active به المان کلیک شده
    if (element) {
        element.classList.add('active');
    }
}
// ۱. تابع دریافت و نمایش محصولات
async function fetchProducts(categoryId, element = null) {
    // اگر المانی پاس داده شده باشد، آن را به حالت انتخاب شده در می‌آوریم
    if (element) {
        setActiveCategory(element);
    }
    const productContainer = document.getElementById('product-list');
    productContainer.innerHTML = '<p>در حال بارگذاری محصولات...</p>';
    try {
        // اگر categoryId خالی باشد، احتمالاً قصد داریم همه محصولات را بگیریم
        let url = `https://localhost:7061/api/Category/GetProducts`;
        if (categoryId) {
            url += `?catid=${categoryId}`;
        }
        const response = await fetch(url);
        const result = await response.json();
        const products = result.data || result;
        if (products && products.length > 0) {
            productContainer.innerHTML = products.map(product => `
                 <div class="product-card">
        <div class="product-image" onclick="showProductDetail(${product.id})" style="cursor: pointer;">
            <img src="https://localhost:7061/${product.src}" onerror="this.src='placeholder.jpg'" />
        </div>
                    <div class="product-info">
                        <h3>${product.name}</h3>
                        <p class="price">${product.price.toLocaleString()} تومان</p>
                        <button onclick="addToCart(${product.id}, '${product.name.replace(/'/g, "\\'")}', ${product.price})">
                            افزودن به سبد
                        </button>
                    </div>
                </div>
            `).join('');
        } else {
            productContainer.innerHTML = '<p>محصولی در این دسته‌بندی یافت نشد.</p>';
        }
    } catch (err) {
        console.error("خطا در دریافت محصولات:", err);
        productContainer.innerHTML = '<p>خطا در برقراری ارتباط با سرور.</p>';
    }
}
// ۲. رندر نوار منو
async function renderNavbar() {
    try {
        const response = await fetch('https://localhost:7061/api/Category/Index');
        const result = await response.json();
        if (result.isSuccess) {
            const container = document.getElementById('main-category-container');
            if (!container) return;
            container.className = "ok-cat-navbar";
            container.innerHTML = '';
            // اصلاح دکمه اصلی: با کلیک روی "دسته‌بندی کالاها" صفحه رفرش می‌شود یا همه محصولات لود می‌شوند
            const mainTriggerHtml = `
                <div class="ok-item">
                    <a href="javascript:void(0)" onclick="location.reload()" class="ok-cat-nav-item ok-cat-bold">
                        <i class="icon-menu"></i>
                        <span>دسته‌بندی کالاها</span>
                    </a>
                </div>
                <div class="ok-cat-divider"></div>
            `;
            container.insertAdjacentHTML('beforeend', mainTriggerHtml);
            result.data.forEach(cat => {
                const wrapper = document.createElement('div');
                wrapper.className = 'ok-item';
                // ارسال this به تابع برای تشخیص المان کلیک شده
                let itemContent = `
                    <a href="javascript:void(0)" onclick="fetchProducts(${cat.id}, this)" class="ok-cat-nav-item">
                        <span>${cat.name}</span>
                    </a>
                `;
                if (cat.hasChildern && cat.subCategory && cat.subCategory.length > 0) {
                    itemContent += `<ul class="ok-dropdown">`;
                    cat.subCategory.forEach(sub => {
                        itemContent += `
                            <li>
                                <a href="javascript:void(0)" onclick="fetchProducts(${sub.id}, this)">${sub.name}</a>
                            </li>
                        `;
                    });
                    itemContent += `</ul>`;
                }
                wrapper.innerHTML = itemContent;
                container.appendChild(wrapper);
            });
        }
    } catch (err) {
        console.error("خطا در لود منو:", err);
    }
}
document.addEventListener('DOMContentLoaded', renderNavbar);

