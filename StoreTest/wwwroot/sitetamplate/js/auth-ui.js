document.addEventListener("DOMContentLoaded", () => {
    const authLinks = document.getElementById("authLinks");
    const userPanel = document.getElementById("userPanel");
    const logoutBtn = document.getElementById("logoutBtn");
    const token = localStorage.getItem("token");
    if (token) {
        if (authLinks) authLinks.style.display = "none";
        if (userPanel) userPanel.style.display = "block";
    } else {
        if (authLinks) authLinks.style.display = "block";
        if (userPanel) userPanel.style.display = "none";
    }
    if (logoutBtn) {
        logoutBtn.addEventListener("click", function (e) {
            e.preventDefault();
            localStorage.removeItem("token");
            localStorage.removeItem("user");
            window.location.href = "index.html";
        });
    }
});
