$(document).ready(function () {
    alert("load wizard");
    $("#example-basic").steps({
        headerTag: "h3",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        autoFocus: true,
        titleTemplate: '<span class="step">#index#</span> #title#',
        labels: {
            finish: 'ذخیره <i  class="ft-check"></i> ',
            previous: 'قبلی <i class="ft-chevron-right"></i>',
            next:'بعدی <i class="ft-chevron-left"></i>',
        },
        onFinished: function (event, currentIndex) {
            $("#frmDiscount").submit();
        }
    });

    $("#wizard-editdiscount").steps({
        headerTag: "h3",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        autoFocus: true,
        titleTemplate: '<span class="step">#index#</span> #title#',
        labels: {
            finish: 'ویرایش <i  class="ft-check"></i> ',
            previous: 'قبلی <i class="ft-chevron-right"></i>',
            next: 'بعدی <i class="ft-chevron-left"></i>',
        },
        onFinished: function (event, currentIndex) {
            $("#frmDiscount").submit();
        }
    });
    var to, from;
    to = $(".end-date").persianDatepicker({
        inline: false,
        altField: '#EndDate',
        observer: true,
        altFormat: 'HH:mm YYYY/MM/DD ',
        initialValue: false,
        persianDigit: false,
        onSelect: function (unix) {
            to.touched = true;
            if (from && from.options && from.options.maxDate != unix) {
                var cachedValue = from.getState().selected.unixDate;
                from.options = { maxDate: unix };
                if (from.touched) {
                    from.setDate(cachedValue);
                }
            }
        }
    });


    from = $(".start-date").persianDatepicker({
        inline: false,
        altField: '#StartDate',
        altFormat: 'HH:mm YYYY/MM/DD ',
        initialValue: false,
        observer: true,
        persianDigit: false,
        onSelect: function (unix) {
            from.touched = true;
            if (to && to.options && to.options.minDate != unix) {
                var cachedValue = to.getState().selected.unixDate;
                to.options = { minDate: unix };
                if (to.touched) {
                    to.setDate(cachedValue);
                }
            }
        }
    });

    $('#UsePointCo').change(function () {
        if (this.checked) {
            $('#PointCo').fadeIn('slow');
            EnableCoFirst = true;

        }
        else {

            $('#PointCo').fadeOut('slow');
            EnableCoFirst = false;
            $('#RcoFirst').text("");
            $('#RcoLast').text("");
            $('#RcoPoint').val("");
        }

    });

    $('#UsePercent').change(function () {
        if (this.checked) {
            $('#Percent').fadeIn('slow');
        }
        else
            $('#Percent').fadeOut('slow');
        $('#RopPercent').text("");
        $('#PercentInput').val("");

    });

    $('#UsePointOp').change(function () {
        if (this.checked) {
            $('#PointOp').fadeIn('slow');
        }
        else
            $('#PointOp').fadeOut('slow');
        $('#RopPoint').text("");
        $('#PointOpInput').val("");
    });
    $('#UseAmountDi').change(function () {
        if (this.checked)
            $('#AmountDi').fadeIn('slow');
        else
            $('#AmountDi').fadeOut('slow');
        $('#AmountDiInput').val("");
        $('#RopAmount').text("");
    });


    $('#InputUseDiscountProducts').change(function () {

        if (this.checked) {
            $('#RcoProduct').text(".در ادامه بعد از ذخیره به صفحه انتخاب محصول و دسته  برای اعمال به تخفیف انتقال داده می شوید");
        }
        else {
            $('#RcoProduct').text();

        }

    });


    $('#InputUseDiscountCategories').change(function () {

        if ($('#RcoProduct').text() == '') {


            if (this.checked) {
                $('#RcoCategory').text(".در ادامه بعد از ذخیره به صفحه انتخاب محصول و دسته  برای اعمال به تخفیف انتقال داده می شوید");
            }
            else {
                $('#RcoCategory').text();

            }
        }
    });


    $('#UseTotalPrice').change(function () {
        if (this.checked) {
            EnableCoFirst = true;
            $('#TotlaPrice').fadeIn('slow');
        }
        else {

            $('#TotlaPrice').fadeOut('slow');
            EnableCoFirst = false;
            $('#RcoFirst').text("");
            $('#RcoLast').text("");
            $('#RcoTotalPrice').val("");
        }

    });

    $('#UseNumberUse').change(function () {
        if (this.checked) {
            EnableCoFirst = true;
            $('#NumberUse').fadeIn('slow');
        }

        else {
            $('#NumberUse').fadeOut('slow');
            EnableCoFirst = false;
            $('#RcoFirst').text("");
            $('#RcoLast').text("");
            $('#RcoNumberUse').val("");
        }


    });



    var EnableCoFirst = false;

    $('#Name').keyup(function () {
        $('#Rname').text("تخفیف با نام" + " " + $(this).val());
    });

    $('#StartDate').on('input', function () {
        $('#RdateS').text("از تاریخ : " + " " + $(this).val());
    });

    $('#EndDate').on('input', function () {
        $('#RdateE').text("تا تاریخ  : " + " " + $(this).val());
    });

    $('#PercentInput').keyup(function () {
        $('#RopPercent').text("با" + " " + $(this).val() + " " + "درصد تخفیف");
    });

    $('#AmountDiInput').keyup(function () {
        $('#RopAmount').text("،" + " " + $(this).val() + " " + "ریال تخفیف");
    });
    $('#PointOpInput').keyup(function () {
        $('#RopPoint').text("،" + " " + $(this).val() + " " + "امتیاز");
    });
    //----شرط ها---
    $('#PointCoInput').keyup(function () {
        $('#RcoPoint').text("امتیاز آنها برابر" + " " + $(this).val() + " " + "");
        if (EnableCoFirst == true) {
            $('#RcoFirst').text("تنها برای کاربرانی فعال است که ");
            $('#RcoLast').text("است اعمال می شود.");
        }
    });

    $('#TotlaPriceInput').keyup(function () {
        $('#RcoTotalPrice').text("،" + "مجموع سفارش آنها برابر " + " " + $(this).val() + " " + "");
        if (EnableCoFirst == true) {
            $('#RcoFirst').text("تنها برای کاربرانی فعال است که ");
            $('#RcoLast').text("است اعمال می شود.");
        }
    });
    $('#NumberUseInput').keyup(function () {
        $('#RcoNumberUse').text("،" + "تعداد خرید آنها از فروشگاه شما برابر یا بیشتر از " + " " + $(this).val() + " " + "");
        if (EnableCoFirst == true) {
            $('#RcoFirst').text("تنها برای کاربرانی فعال است که ");
            $('#RcoLast').text("است اعمال می شود.");

        }
    });



    if (document.getElementById("UsePercent").checked != true) {
        $('#Percent').hide();
    }
    if (document.getElementById("UseNumberUse").checked != true) {
        $('#NumberUse').hide();
    }
    if (document.getElementById("UseTotalPrice").checked != true) {
        $('#TotlaPrice').hide();
    }

    if (document.getElementById("UsePointCo").checked != true) {
        $('#PointCo').hide();
    }
    if (document.getElementById("UseAmountDi").checked != true) {
        $('#AmountDi').hide();
    }
    if (document.getElementById("UsePointOp").checked != true) {
        $('#PointOp').hide();
    }
});