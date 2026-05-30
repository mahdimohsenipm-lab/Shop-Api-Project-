

const LOGIN_API_URL = "https://localhost:7061/api/User/Login";
document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.getElementById("loginForm");
    const messageElement = document.getElementById("loginMessage");
    if (!loginForm) return;
    loginForm.addEventListener("submit", async function (e) {
        e.preventDefault();
        const email = document.getElementById("loginEmail").value.trim();
        const password = document.getElementById("loginPassword").value;
        messageElement.textContent = "";
        messageElement.style.color = "red";
        if (!email || !password) {
            messageElement.textContent = "لطفاً ایمیل و رمز عبور را وارد کنید.";
            return;
        }
        const requestBody = {
            email: email,
            password: password
        }; 
        try {
            const response = await fetch(LOGIN_API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            });
            const result = await response.json();
            if (response.ok && result.isSuccess) {
                if (result.data && result.data) {
                    localStorage.setItem("token", result.data);
                }
                if (result.data && result.data.user) {
                    localStorage.setItem("user", JSON.stringify(result.data.user));
                }
                messageElement.style.color = "green";
                messageElement.textContent = result.message || "ورود با موفقیت انجام شد.";
                setTimeout(() => {
                    window.location.href = "index.html";
                }, 1000);
            } else {
                messageElement.style.color = "red";
                messageElement.textContent = result.message || "ایمیل یا رمز عبور اشتباه است.";
            }
        } catch (error) {
            console.error("خطا در ورود:", error);
            messageElement.style.color = "red";
            messageElement.textContent = "خطا در ارتباط با سرور.";
        }
    });
});
