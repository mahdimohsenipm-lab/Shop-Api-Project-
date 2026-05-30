const REGISTER_API_URL = "https://localhost:7061/api/User/Register";
document.addEventListener("DOMContentLoaded", () => {
    const registerForm = document.getElementById("registerForm");
    const messageElement = document.getElementById("registerMessage");
    if (!registerForm) return;
    registerForm.addEventListener("submit", async function (e) {
        e.preventDefault();
        const CustomerName = document.getElementById("fullName").value.trim();
        const email = document.getElementById("email").value.trim();
        const age = document.getElementById("age").value.trim();
        const customerName = document.getElementById("customerName").value.trim();

        const password = document.getElementById("password").value;
        const confirmPassword = document.getElementById("confirmPassword").value;
        messageElement.textContent = "";
        messageElement.style.color = "red";
        if (!CustomerName || !email || !age || !password || !confirmPassword || !customerName) {
            messageElement.textContent = "لطفاً همه فیلدها را پر کنید.";
            return;
        }
        if (password !== confirmPassword) {
            messageElement.textContent = "رمز عبور و تکرار آن یکسان نیست.";
            return;
        }
        const requestBody = {
            userName: CustomerName,
            fullName: customerName,
            email: email,
            age: Number(age),
            password: password
        };
        try {
            const response = await fetch(REGISTER_API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            });
            const result = await response.json();
            console.log("Register Result:", result);
            if (response.ok && result.isSuccess) {
                const token = result.token || result.data?.token;
                if (token) {
                    localStorage.setItem("token", token);
                }
                if (result.user) {
                    localStorage.setItem("user", JSON.stringify(result.user));
                } else if (result.data?.user) {
                    localStorage.setItem("user", JSON.stringify(result.data.user));
                }
                messageElement.style.color = "green";
                messageElement.textContent = result.message || "ثبت‌نام با موفقیت انجام شد.";
                setTimeout(() => {
                    window.location.href = "index.html";
                }, 1000);
            } else {
                messageElement.style.color = "red";
                if (result.errors) {
                    const firstErrorKey = Object.keys(result.errors)[0];
                    const firstError = result.errors[firstErrorKey]?.[0];
                    messageElement.textContent = firstError || "ثبت‌نام انجام نشد.";
                } else {
                    messageElement.textContent = result.message || "ثبت‌نام انجام نشد.";
                }
            }
        } catch (error) {
            console.error("خطا در ثبت‌نام:", error);
            messageElement.style.color = "red";
            messageElement.textContent = "خطا در ارتباط با سرور.";
        }
    });
});
