// =======================
// 🛒 سبد خرید
// =======================
function isUserLoggedIn() {
    return !!localStorage.getItem("token");
    // اگر توکن داری:
    // return !!localStorage.getItem("token");
}
function goToCheckout() {
    if (isUserLoggedIn()) {
        window.location.href = "checkout.html";
    } else {
        window.location.href = "login.html";
    }
}
let cart = JSON.parse(localStorage.getItem("cart")) || [];
// افزودن محصول به سبد
function addToCart(id, name, price) {
    const existingItem = cart.find(item => item.id === id);
    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({ id, name, price, quantity: 1 });
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    displayCart();
}
// حذف محصول
function removeItem(id) {
    cart = cart.filter(item => item.id !== id);
    localStorage.setItem("cart", JSON.stringify(cart));
    displayCart();
}
// تغییر تعداد
function changeQuantity(id, change) {
    id = Number(id);
    const item = cart.find(product => product.id === id);
    if (!item) return;
    item.quantity += change;
    if (item.quantity <= 0) {
        cart = cart.filter(product => product.id !== id);
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    displayCart();
}
// نمایش سبد
function displayCart() {
    const cartItems = document.getElementById("cart-items");
    const totalElement = document.getElementById("total");
    if (!cartItems || !totalElement) return;
    cartItems.innerHTML = "";
    if (cart.length === 0) {
        cartItems.innerHTML = `
            <p style="text-align:center; padding:20px; color:#888;">
                سبد خرید خالی است 🛒
            </p>
        `;
        totalElement.textContent = "0 تومان";
        return;
    }
    let total = 0;
    cart.forEach(item => {
        total += item.price * item.quantity;
        const li = document.createElement("li");
        li.classList.add("cart-item");
        li.innerHTML = `
            <div class="cart-item-info">
                <span class="cart-item-name">${item.name}</span>
                <span class="cart-item-price">
                    ${(item.price * item.quantity).toLocaleString()} تومان
                </span>
            </div>
            <div class="quantity-control">
                <button class="qty-btn minus" onclick="changeQuantity(${item.id}, -1)">−</button>
                <span class="qty-number">${item.quantity}</span>
                <button class="qty-btn plus" onclick="changeQuantity(${item.id}, 1)">+</button>
                <button class="remove-btn" onclick="removeItem(${item.id})">🗑</button>
            </div>
        `;
        cartItems.appendChild(li);
    });
    totalElement.textContent = total.toLocaleString() + " تومان";
}
// =======================
// 🚀 اجرای اولیه صفحه
// =======================
document.addEventListener("DOMContentLoaded", async () => {
    try {
        const response = await fetch("https://localhost:7061/api/Products");
        const result = await response.json();
        const products = result.data.products;

        const container = document.getElementById("product-list");

        if (container) {
            container.innerHTML = products.map(product => `
                <div class="product-card">
                    <div class="product-image"
                         onclick="showProductDetail(${product.id})"
                         style="cursor: pointer;">

                        <img src="https://localhost:7061/${product.src}"
                             onerror="this.src='placeholder.jpg'" />
                    </div>

                    <div class="product-info">

                        <h3 onclick="showProductDetail(${product.id})"
                            style="cursor: pointer;">
                            ${product.name}
                        </h3>

                ${product.price > product.finalPrice
                    ? `
        <div class="price-box">
            <span class="old-price">
                ${product.price.toLocaleString()} تومان
            </span>

            <div class="discount-row">
                <span class="final-price">
                    ${product.finalPrice.toLocaleString()} تومان
                </span>

                <span class="discount-badge">
                    ${product.percentage != 0
                        ? `${product.percentage}% تخفیف`
                        : `${product.amount.toLocaleString()} تومان تخفیف`
                    }
                </span>
            </div>
        </div>
    `
                    : `
        <p class="price">
            ${product.price.toLocaleString()} تومان
        </p>
    `
}

                        <button
                            onclick="event.stopPropagation(); addToCart(${product.id}, '${product.name.replace(/'/g, "\\'")}', ${product.finalPrice})">
                            افزودن به سبد
                        </button>

                    </div>
                </div>
            `).join('');
        }

    } catch (error) {
        console.error("خطا در دریافت محصولات:", error);
    }

    const checkoutBtn = document.getElementById("checkout-btn");

    if (checkoutBtn) {
        checkoutBtn.addEventListener("click", goToCheckout);
    }

    displayCart();
});
