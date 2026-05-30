$("#logout").on("click", function (e) {
    e.preventDefault();
    $.ajax({
        url: '/Admin/Authentication/Logout',
        type: 'POST',
        success: function (response) {
            window.location.href = '/Admin/Authentication/Login';
        },
        error: function (xhr, status, error) {
            console.error('Logout Error:', error);
            alert('خطا در خروج از حساب');
        }
    });
});
console.log("register js loaded");

