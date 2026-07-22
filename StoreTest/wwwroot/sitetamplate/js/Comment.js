const baseUrl = "https://localhost:7061/";
let currentComments = [];

async function loadComments(productId) {

    const container = document.getElementById("commentsContainer");

    container.innerHTML = `
        <div class="loading-comments">
            در حال دریافت دیدگاه‌ها...
        </div>
    `;

    try {

        const response = await fetch(
            `${baseUrl}api/Comment/Get?productId=${productId}`
        );

        if (!response.ok)
            throw new Error("خطا در دریافت اطلاعات");

        const json = await response.json();

        const result = json.data;

        renderCommentSummary(result);

        renderComments(result.comments);

        initializeCommentEvents();

    }
    catch (error) {

        console.error(error);

        container.innerHTML = `
        <div class="error-comments">
            خطا در دریافت دیدگاه‌ها
        </div>
    `;
    }

}

function renderCommentSummary(data) {

    const averageRate = Number(data.averageRate ?? 0);

    document.getElementById("averageRate").innerText =
        averageRate.toFixed(1);

    document.getElementById("totalComments").innerText =
        `${data.totalComment} دیدگاه`;

    renderAverageStars(averageRate);
}

function renderAverageStars(rate) {

    let html = "";

    for (let i = 1; i <= 5; i++) {

        html += i <= Math.round(rate)
            ? "★"
            : "☆";

    }

    document.getElementById("averageStars").innerHTML = html;

}

function renderComments(comments) {

    currentComments = comments;

    const container = document.getElementById("commentsContainer");

    if (comments.length === 0) {
        container.innerHTML = `
            <div class="empty-comments">
                هنوز نظری ثبت نشده است.
            </div>
        `;
        return;
    }
    console.log("renderComments");
    console.log(comments);

    container.innerHTML = comments.map(renderComment).join("");
}



function renderComment(comment) {

    return `

    <div class="comment-item">

        <div class="comment-header">

            <div>

                <div class="comment-user">
                    ${comment.fullName}
                </div>

                <div class="comment-date">
                    ${formatDate(comment.createTime)}
                </div>

            </div>

        </div>

        <div class="comment-rate">
            ${createStars(comment.rate)}
        </div>

        <div class="comment-text">
            ${comment.text}
        </div>

        <div class="comment-actions">

            <button
                class="reply-btn"
                data-id="${comment.id}">
                پاسخ
            </button>

        </div>

        <div
            class="reply-box"
            id="reply-box-${comment.id}"
            style="display:none">

            <textarea
                id="reply-text-${comment.id}"
                placeholder="پاسخ خود را بنویسید...">
            </textarea>

            <button
                class="send-reply-btn"
                data-id="${comment.id}">
                ارسال پاسخ
            </button>

        </div>

        ${renderReplies(comment.replies, comment.id)}

    </div>

    `;
}

function renderReplies(replies, commentId) {

    if (!replies || replies.length === 0)
        return "";

    const visibleReplies = replies.slice(0, 2);

    return `

        <div
            class="reply-list"
            id="reply-list-${commentId}">

            ${visibleReplies.map(renderReplyItem).join("")}

        </div>

        ${replies.length > 2 ? `

           <button
    class="toggle-replies-btn"
    data-id="${commentId}"
    data-open="false">

                نمایش ${replies.length - 2} پاسخ دیگر

            </button>

        ` : ""}

    `;

}


function createStars(rate) {

    let html = "";

    for (let i = 1; i <= 5; i++) {

        html += i <= rate
            ? "★"
            : "☆";

    }

    return html;

}

function formatDate(date) {

    return new Date(date)
        .toLocaleDateString("fa-IR");

}



let selectedRate = 5;

function initializeCommentEvents() {

    const stars = document.querySelectorAll(".rate-star");

    stars.forEach(star => {

        star.addEventListener("click", function () {

            selectedRate = Number(this.dataset.rate);

            updateSelectedStars();

        });

    });

    const submitButton =
        document.getElementById("submitComment");

    if (submitButton) {

        submitButton.onclick = function () {

            submitComment();

        };

    }

    updateSelectedStars();

}


//document.addEventListener("click", function (e) {

//    if (e.target.classList.contains("reply-btn")) {

//        const id = e.target.dataset.id;

//        document.querySelectorAll(".reply-box")
//            .forEach(x => x.style.display = "none");

//        document
//            .getElementById(`reply-box-${id}`)
//            .style.display = "block";

//    }

//});


async function submitReply(parentId, text) {

    const token = localStorage.getItem("token");

    if (!token) {

        Swal.fire({
            icon: "warning",
            text: "ابتدا وارد حساب کاربری شوید."
        });

        return;
    }

    const productId = Number(new URLSearchParams(window.location.search).get("id"));

    try {

        const response = await fetch(`${baseUrl}api/Comment/Add`, {

            method: "POST",

            headers: {

                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`

            },

            body: JSON.stringify({

                productId: productId,

                rate: 5,

                text: text,

                parentId: parentId

            })

        });

        if (!response.ok)
            throw new Error();

        Swal.fire({

            icon: "success",

            title: "پاسخ ثبت شد",

            timer: 1500,

            showConfirmButton: false

        });

        loadComments(Number(new URLSearchParams(window.location.search).get("id")));

    }

    catch {

        Swal.fire({

            icon: "error",

            title: "خطا در ثبت پاسخ"

        });

    }

}


function updateSelectedStars() {

    const stars = document.querySelectorAll(".rate-star");

    stars.forEach(star => {

        const rate = Number(star.dataset.rate);

        if (rate <= selectedRate)
            star.classList.add("active");
        else
            star.classList.remove("active");

    });

}



async function submitComment(parentId = null) {

    const token = localStorage.getItem("token");

    if (!token) {

        alert("ابتدا وارد حساب کاربری شوید.");

        return;

    }

    const text =
        document.getElementById("commentText").value.trim();

    if (text.length === 0) {

        alert("متن دیدگاه را وارد کنید.");

        return;

    }

    try {


  

   

        const response = await fetch(`${baseUrl}api/Comment/Add`, {

            method: "POST",

            headers: {

                "Content-Type": "application/json",

                "Authorization": `Bearer ${token}`

            },

            body: JSON.stringify({

                productId: Number(new URLSearchParams(window.location.search).get("id")),

                rate: selectedRate,

                text: text,

                parentId: parentId

            })

        });

        if (!response.ok) {

            const error = await response.text();

            throw new Error(error);

        }

        const message = await response.text();

        alert(message);

        document.getElementById("commentText").value = "";

        selectedRate = 5;

        updateSelectedStars();

        loadComments(Number(new URLSearchParams(window.location.search).get("id")));

    }

    catch (error) {

        console.error(error);

        alert("ثبت دیدگاه با خطا مواجه شد.");

    }

}

document.addEventListener("click", async function (e) {

    // باز کردن فرم پاسخ

    if (e.target.closest(".reply-btn")) {

        const id = e.target.closest(".reply-btn").dataset.id;

        document
            .querySelectorAll(".reply-box")
            .forEach(x => x.style.display = "none");

        document
            .getElementById(`reply-box-${id}`)
            .style.display = "block";

        return;
    }

    // ارسال پاسخ

    if (e.target.closest(".send-reply-btn")) {

        const parentId =
            Number(e.target.closest(".send-reply-btn").dataset.id);

        const text =
            document
                .getElementById(`reply-text-${parentId}`)
                .value
                .trim();

        if (!text) {

            Swal.fire({

                icon: "warning",

                text: "متن پاسخ را وارد کنید."

            });

            return;

        }

        await submitReply(parentId, text);

    }

});



function renderReplyItem(reply) {

    return `

        <div class="reply-item">

            <div class="reply-user">

                ${reply.fullName}

            </div>

            <div class="reply-date">

                ${formatDate(reply.createTime)}

            </div>

            <div class="reply-text">

                ${reply.text}

            </div>

        </div>

    `;

} document.addEventListener("click", function (e) {

    const btn = e.target.closest(".toggle-replies-btn");
    console.log("click");
    if (!btn)
        return;

    const commentId = Number(btn.dataset.id);

    const comment =
        currentComments.find(x => x.id === commentId);

    if (!comment)
        return;

    const container =
        document.getElementById(`reply-list-${commentId}`);

    const isOpen =
        btn.dataset.open === "true";

    if (!isOpen) {

        container.innerHTML =
            comment.replies
                .map(renderReplyItem)
                .join("");

        btn.dataset.open = "true";

        btn.innerText = "پنهان کردن پاسخ‌ها";

    }

    else {

        container.innerHTML =
            comment.replies
                .slice(0, 2)
                .map(renderReplyItem)
                .join("");

        btn.dataset.open = "false";

        btn.innerText =
            `نمایش ${comment.replies.length - 2} پاسخ دیگر`;

    }

});

