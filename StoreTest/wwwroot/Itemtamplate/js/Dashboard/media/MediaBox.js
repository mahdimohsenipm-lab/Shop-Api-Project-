function MediaBox(type) {
    var entity = type;

var firstLoadImages = false;
var MaxPageId = 1;
var PicturePageId = 0;
var ClickFilterButton = false;


$("#modal").iziModal({
    title: 'گالری تصاویر',
    subtitle: 'امکان انتخاب تصویر از گالری تصاویر و یا آپلود تصویر جدید امکان پذیر است ',
    headerColor: '#88A0B9',
    background: null,
    theme: '',  // light
    icon: 'ft-image',
    iconText: null,
    iconColor: '',
    rtl: true,
    width: 1000,
    top: 50,
    bottom: 50,
    borderBottom: true,
    padding: 0,
    radius: 3,
    zindex: 999,
    iframe: false,
    iframeHeight: 400,
    iframeURL: null,
    focusInput: false,
    group: '',
    loop: false,
    arrowKeys: true,
    navigateCaption: true,
    navigateArrows: true, // Boolean, 'closeToModal', 'closeScreenEdge'
    history: false,
    restoreDefaultContent: false,
    autoOpen: 0, // Boolean, Number
    bodyOverflow: true,
    fullscreen: true,
    openFullscreen: false,
    closeOnEscape: true,
    closeButton: true,
    appendTo: 'body', // or false
    appendToOverlay: 'body', // or false
    overlay: true,
    overlayClose: true,
    overlayColor: 'rgba(0, 0, 0, 0.4)',
    timeout: false,
    timeoutProgressbar: false,
    pauseOnHover: false,
    timeoutProgressbarColor: 'rgba(255,255,255,0.5)',
    transitionIn: 'comingIn',
    transitionOut: 'comingOut',
    transitionInOverlay: 'fadeIn',
    transitionOutOverlay: 'fadeOut',
    onFullscreen: function () { },
    onResize: function () { },
    onOpening: function () {
        //if (!firstLoadImages) {
        //    $.getJSON('/api/Media/Index', function (json) {
        //        var obj = json;
        //        firstLoadImages = true;
        //        for (i in obj.data.result) {
        //            $("#imageBox").append("<img dir=rtl data-html=true data-placement=bottom data-toggle=tooltip title='<p> نام تصویر :  " + obj.data.result[i].pictureTitle + " </p> <p> کد  :  " + obj.data.result[i].id + " </p>  '  name=" + obj.data.result[i].id + " src=" + obj.data.result[i].address + " class=img-box >");
        //        }
        //        BindJsToImages();

        //    });
        //}
        if (!firstLoadImages) {
            GetAndBindPictures('', '', '', null, 1);
            PicturePageId = 1;
        }


    },
    onOpened: function () {


    },
    onClosing: function () { },
    onClosed: function ()
    {
        savePictures();
        IsSavedPictures = true;
    },
    afterRender: function () {
    }

});

function GetAndBindPictures(StartDate, EndDate, Search, Id, PageId = 1) {

    if (PageId > MaxPageId) {
        PicturePageId = PicturePageId - 1;
        return;
    }
    firstLoadImages = true;
    $("#media-loading").show();
    if (ClickFilterButton) {
        $("#btnFilter").html("فیلتر  <i class='fa fa-spinner fa-spin'></i> ");
        $("#btnFilter").attr('disabled', '');
    }
    $.getJSON('/api/Media/Index?StartDate=' + StartDate + "&EndDate=" + EndDate + "&Search=" + Search + "&Id=" + Id + "&PageId=" + PageId,
        function (json) {
            var obj = json;
            var CountPicture = obj.data.count;
            var NumberList = obj.data.numberList;
            if (CountPicture > NumberList) {
                MaxPageId = CountPicture / NumberList;
                if (CountPicture % NumberList != 0) {
                    MaxPageId = MaxPageId + 1;
                }
            }
            for (i in obj.data.result) {
                $("#imageBox").append
                    ("<li><input " + (listId.includes(obj.data.result[i].id) ? 'checked' : '') + "  data-id=" + obj.data.result[i].id + "  class='input-media' type=checkbox id=mediaCheckbox" + obj.data.result[i].id + " /> <label for=mediaCheckbox" + obj.data.result[i].id + "> <img dir=rtl data-html=true data-placement=bottom data-toggle=tooltip title='<p> نام تصویر :  " + obj.data.result[i].pictureTitle + " </p> <p> کد  :  " + obj.data.result[i].id + " </p>  '  name=" + obj.data.result[i].id + " src=" + obj.data.result[i].address +"thumbnail_"+ obj.data.result[i].title + " class=img-box  data-id=" + obj.data.result[i].id + "   </label>  </li>");
            }
            $("#media-loading").hide();
            if (ClickFilterButton) {
                $("#btnFilter").html(" فیلتر  <i class='fa fa-filter'></i>");
                $("#btnFilter").removeAttr('disabled');
            }
            ClickFilterButton = false;
        });
}

    $("#modal .iziModal-wrap").on('scroll', function () {
    let div = $(this).get(0);
    if (div.scrollTop + div.clientHeight >= div.scrollHeight) {
        var filterStartDate = $("#filterStartDate").val();
        var filterEndDate = $("#filterEndDate").val();
        var filterSearch = $("#filterSearch").val();
        var filterId = $("#filterId").val();
        PicturePageId += 1;
        GetAndBindPictures(filterStartDate, filterEndDate, filterSearch, filterId, PicturePageId);
    }
    });
$("#btnFilter").click(function () {
    ClickFilterButton = true;
    PicturePageId = 1;
    $("#imageBox").html('');
    var filterStartDate = $("#filterStartDate").val();
    var filterEndDate = $("#filterEndDate").val();
    var filterSearch = $("#filterSearch").val();
    var filterId = $("#filterId").val();
    GetAndBindPictures(filterStartDate, filterEndDate, filterSearch, filterId, 1);
});
$(document).on("click", "input[class='input-media'][type='checkbox']", function () {
    $("#thumbnail-box").show();
    var id = $(this).attr("data-id");
    if ($(this).is(':checked')) {
        var src = $("img[data-id=" + id + "]").attr("src");
        $("#thumbnail-media").append(" <li data-id=" + id + "> <a href='#' > <img data-id=" + id + " class='img-box' src=" + src + " /> </a> <p> <a  class='remove-thumbnail' data-id=" + id + "  href='#'>حذف  <i class='fa fa-remove'></i> </a> </p> </li>");
        $("#selected-thumbnail-media").append(" <li data-id=" + id + ">  <img data-id=" + id + " class='img-box' src=" + src + " /> </li>");
        if (!listId.includes(listId.includes(parseInt(id))))
            listId.push(parseInt(id));
    }
    else {
        $("li").remove("#thumbnail-media li[data-id=" + id + "]");
        $("li").remove("#selected-thumbnail-media li[data-id=" + id + "]");
        var index = listId.indexOf(parseInt(id));
        if (index > -1) {
            listId.splice(index, 1);
        }
        if (listId.length == 0)
            $("#thumbnail-box").hide();
    }


});

$(document).on("click", "#thumbnail-media li a img", function () {
    $("#thumbnail-media li a img").removeClass("default-media-image");
    $("#selected-thumbnail-media li img").removeClass("default-media-image");

    if ($(this).hasClass("default-media-image")) {
        $(this).removeClass("default-media-image");
       
    }

    else {
        defaultImageId = $(this).attr("data-id");
    }

    $(this).addClass("default-media-image");
    $("ul[id='selected-thumbnail-media'] li img[data-id=" + $(this).attr("data-id") + "]").addClass("default-media-image");

});


$(document).on("click", "a[class='remove-thumbnail']", function () {
    var id = $(this).attr("data-id");
    $("input[class= 'input-media'][type = 'checkbox'][data-id=" + id + "]").prop('checked', false);
    $("li").remove("#thumbnail-media li[data-id=" + id + "]"); 
    $("li").remove("#selected-thumbnail-media li[data-id=" + id + "]");
    var index = listId.indexOf(parseInt(id));
    if (index > -1) {
        listId.splice(index, 1);
    }
    if (listId.length == 0)
        $("#thumbnail-box").hide();
});

$(document).on('click', '.trigger', function (event) {
    event.preventDefault();
    $('#modal').iziModal('open');
    });
    var MultiImageUploadBox = new Dropzone("div#MultiImageUploadBox", {
        url: "/api/Media/UploadFile?Id=" + ProductId + "&Type=" + entity,
        acceptedFiles: "image/*",
        maxFilesize: 6, // MB
        maxFiles: 10,
        success: function (file, response) {
            $("a[class='dz-remove'][data-name='" + file.name + "']").attr("data-id", response.data.id);


        },
        init: function () {
            this.on("addedfile", function (file) {
                // Create the remove button

                var removeButton = Dropzone.createElement("<a  class='dz-remove' data-name='" + file.name + "'  data-id='' > حذف </a>");
                var _this = this;

                removeButton.addEventListener("click", function (e) {
                    var id = $(this).attr("data-id");
                    $.ajax({
                        type: "POST",
                        url: "/api/Media/Delete?Id=" + id,
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {


                        },
                        failure: function (response) {


                        },
                        error: function (response) {

                        }
                    });
                    e.preventDefault();
                    e.stopPropagation();
                    _this.removeFile(file);
                });

                file.previewElement.appendChild(removeButton);
            });
        }
    });
}