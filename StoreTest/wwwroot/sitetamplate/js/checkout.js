document.addEventListener("DOMContentLoaded", () => {
    const cart = JSON.parse(localStorage.getItem("cart")) || [];
    const cartContainer = document.getElementById("checkout-cart-items");
    const totalElement = document.getElementById("checkout-total");
    const totalItemsElement = document.getElementById("total-items");
    const form = document.getElementById("checkoutForm");
    const messageElement = document.getElementById("checkoutMessage");

    const applyDiscountBtn = document.getElementById("applyDiscountBtn");
    const discountMessage = document.getElementById("discountMessage");
    const discountAmount = document.getElementById("discountAmount");

    let appliedDiscount = null;
    // ۱. نمایش آیتم‌های سبد خرید در صفحه
    function displayCheckoutItems() {
        if (!cartContainer) return;
        if (cart.length === 0) {
            cartContainer.innerHTML = `<div class="empty-cart">سبد خرید شما خالی است.</div>`;
            if (totalElement) totalElement.textContent = "0 تومان";
            if (totalItemsElement) totalItemsElement.textContent = "0";
            return;
        }
        let totalPrice = 0;
        let totalCount = 0;
        cartContainer.innerHTML = "";
        cart.forEach(item => {
            const itemTotal = item.price * item.quantity;
            totalPrice += itemTotal;
            totalCount += item.quantity;
            const itemDiv = document.createElement("div");
            itemDiv.classList.add("checkout-item");
            itemDiv.innerHTML = `
                <div class="checkout-item-info">
                    <span class="checkout-item-name">${item.name}</span>
                    <span class="checkout-item-details">
                        تعداد: ${item.quantity} × ${item.price.toLocaleString()} تومان
                    </span>
                </div>
                <div class="checkout-item-price">
                    ${itemTotal.toLocaleString()} تومان
                </div>
            `;
            cartContainer.appendChild(itemDiv);
        });
        if (totalElement) totalElement.textContent = totalPrice.toLocaleString() + " تومان";
        if (totalItemsElement) totalItemsElement.textContent = totalCount;
    }
    displayCheckoutItems();

    applyDiscountBtn.addEventListener("click", applyDiscount);
    // ۲. مدیریت ارسال فرم و هدایت به درگاه پرداخت
    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        // دریافت مقادیر فرم
        const fullName = document.getElementById("fullName")?.value.trim();
        const phone = document.getElementById("phone")?.value.trim();
        const address = document.getElementById("address")?.value.trim();
        const note = document.getElementById("note")?.value.trim() || "بدون توضیحات";
        // اعتبارسنجی
        if (cart.length === 0) {
            showMessage("سبد خرید شما خالی است.", "red");
            return;
        }
        if (!fullName || !phone || !address) {
            showMessage("لطفاً تمامی فیلدهای ضروری را پر کنید.", "red");
            return;
        }
        const token = localStorage.getItem("token");
        if (!token) {
            showMessage("ابتدا باید وارد حساب کاربری خود شوید.", "orange");
            setTimeout(() => { window.location.href = "login.html"; }, 2000);
            return;
        }
        // آماده‌سازی داده‌ها برای اکشن Index (شروع پرداخت)
        const orderData = {
            address: address,
            phoneNumber: phone, // مطابق با نیاز مدل C# شما
            fullName: fullName,
            note: note,
            discountCode: document.getElementById("discountCode").value.trim(),
            items: cart.map(item => ({
                ProductId: item.id,
                price: item.price,
                count: item.quantity
            }))
        }; 
        try {
            showMessage("د�� حال انتقال به درگاه پرداخت...", "blue");
            // ارسال درخواست به اکشن Index برای دریافت لینک درگاه
            const response = await fetch("https://localhost:7061/api/Pay/Index", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(orderData)
            });
            // بررسی نوع پاسخ
            const contentType = response.headers.get("content-type");
            if (!contentType || !contentType.includes("application/json")) {
                const errorText = await response.text();
                console.error("Server Error Response:", errorText);
                throw new Error("پاسخ سرور معتبر نیست (احتمالا خطای داخلی سرور).");
            }
          
            const result = await response.json();
            if (response.ok && result.isSuccess) {
                // دریافت آدرس درگاه از خروجی API
                const redirectUrl = result.data?.redirectUrl || result.data;
                if (redirectUrl) {
                    // انتقال کاربر به سایت زرین‌پال
                    window.location.href = redirectUrl;
                } else {
                    showMessage("خطا: آدرس درگاه دریافت نشد.", "red");
                }
            } else {
                showMessage(result.message || "خطا در ثبت سفارش", "red");
            }
        } catch (error) {
            console.error("Error details:", error);
            showMessage("اتصال به سرور برقرار نشد. لطفا بعدا تلاش کنید.", "red");
        }
    });
    // تابع کمکی برای نمایش پیام
    function showMessage(text, color) {
        if (!messageElement) return;
        messageElement.style.color = color;
        messageElement.textContent = text;
    }




    async function applyDiscount() {

        const token = localStorage.getItem("token");

        if (!token) {
            discountMessage.style.color = "red";
            discountMessage.textContent = "ابتدا وارد حساب کاربری شوید.";
            return;
        }

        const code = document.getElementById("discountCode").value.trim();

        if (!code) {
            discountMessage.style.color = "red";
            discountMessage.textContent = "کد تخفیف را وارد کنید.";
            return;
        }

        const request = {
            code: code,
            items: cart.map(item => ({
                productId: item.id,
                count: item.quantity
            }))
        };

        try {

            const response = await fetch("https://localhost:7061/api/Discount/Apply", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(request)
            });

            const result = await response.json();

            if (!response.ok || !result.isSuccess) {

                appliedDiscount = null;

                discountMessage.style.color = "red";
                discountMessage.textContent =
                    result.message || "کد تخفیف معتبر نیست.";

                discountAmount.textContent = "0 تومان";

                return;
            }

            appliedDiscount = result.data;

            discountMessage.style.color = "green";

            if (appliedDiscount.discountPercrntage > 0) {
                discountMessage.textContent =
                    `کد تخفیف اعمال شد (${appliedDiscount.discountPercrntage}% تخفیف)`;
            } else {
                discountMessage.textContent =
                    "کد تخفیف اعمال شد";
            }

            discountAmount.textContent =
                appliedDiscount.discountAmount.toLocaleString() + " تومان";

            totalElement.textContent =
                appliedDiscount.finalPrice.toLocaleString() + " تومان";

        }
        catch (error) {

            console.error(error);

            discountMessage.style.color = "red";
            discountMessage.textContent =
                "خطا در ارتباط با سرور";
        }
    }
});

